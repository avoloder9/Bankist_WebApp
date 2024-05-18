using API.Data;
using API.Data.Models;
using API.Endpoints.TransactionEndpoints.Execute;
using API.Helper.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using API.Helper.Auth;
using Microsoft.AspNetCore.SignalR;
using API.SignalR;
using Microsoft.Extensions.Caching.Memory;
using API.ViewModels;
using API.Helper;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class TransactionController : ControllerBase
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly IMemoryCache _cache;
        private readonly LoyaltyService _loyaltyService;
        public TransactionController(ApplicationDbContext dbContext, MyAuthService authService, IHttpContextAccessor httpContextAccessor, IHubContext<SignalRHub> hubContext, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
            _cache = cache;
            _loyaltyService = new LoyaltyService(_dbContext, _hubContext);
        }

        [HttpGet("user-transaction")]
        public ActionResult<List<Transaction>> GetUserTransaction([FromQuery] string bankName)
        {
            if (!_authService.IsLogin())
            {
                Response.StatusCode = 401;
            }

            string? authToken = _httpContextAccessor.HttpContext!.Request.Headers["Token"];
            if (string.IsNullOrEmpty(authToken))
            {
                return Unauthorized("Unauthorized");
            }
            try
            {
                var databaseToken = _dbContext.AutentificationToken.Where(token => token.value == authToken).ToList();
                var user = databaseToken.Any() ? databaseToken[0].account.User : new User();

                var userExistsInBank = _dbContext.BankUserCard
           .Any(buc => buc.userId == user.id && buc.bank.username == bankName);

                if (!userExistsInBank)
                {
                    return BadRequest($"User with ID {user.id} does not have an account in the bank with ID {bankName}.");
                }

                var userCardIds = _dbContext.BankUserCard
                                   .Where(buc => buc.userId == user.id && buc.bank.username == bankName)
                                   .Select(buc => buc.cardId)
                                   .ToList();

                var transactions = _dbContext.Transaction.Include(t => t.senderCard.currency).Include(t => t.recieverCard.currency)
                                   .Where(t =>
                                       (t.senderCardId.HasValue && userCardIds.Contains(t.senderCardId.Value)) ||
                                       userCardIds.Contains(t.recieverCardId))
                                   .OrderBy(t => t.transactionId)
                                   .ToList();

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("bank-transaction")]
        public ActionResult<List<Transaction>> GetBankTransaction(int bankId)
        {
            try
            {
                var validCardIds = _dbContext.BankUserCard
                                              .Where(buc => buc.bankId == bankId)
                                              .Select(buc => buc.cardId)
                                              .ToList();

                var transactions = _dbContext.Transaction
                                             .Include(x => x.senderCard)
                                             .ThenInclude(x => x.currency)
                                             .Include(x => x.recieverCard)
                                             .ThenInclude(x => x.currency)
                                             .Where(t => validCardIds.Contains(t.senderCardId ?? 0) ||
                                                         validCardIds.Contains(t.recieverCardId))
                                             .OrderByDescending(t => t.transactionDate)
                                             .ToList();

                if (!transactions.Any())
                {
                    return NotFound($"No transactions found for bank with ID {bankId}.");
                }

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("execute")]
        public async Task<ActionResult<TransactionExecuteDetailVM>> ExecuteTransaction([FromBody] TransactionExecuteVM request)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            Transaction transactionRecord = null;
            try
            {
                var senderCard = await _dbContext.Card.Include(c => c.cardType).Include(c => c.currency).FirstOrDefaultAsync(c => c.cardNumber == request.senderCardId);
                var receiverCard = await _dbContext.Card.Include(c => c.cardType).Include(c => c.currency).FirstOrDefaultAsync(c => c.cardNumber == request.recieverCardId);

                if (senderCard == null || receiverCard == null)
                {
                    return BadRequest("Invalid card number");
                }

                if (senderCard.amount < request.amount)
                {
                    return StatusCode(401, "Insufficient funds");
                }

                var blockedCard = _dbContext.BankUserCard.FirstOrDefault(buc => buc.cardId == senderCard.cardNumber);
                if (blockedCard.isBlock == true)
                {
                    throw new Exception("Your card is blocked");
                }

                var senderBank = await _dbContext.BankUserCard.Include(buc => buc.bank).FirstOrDefaultAsync(buc => buc.cardId == senderCard.cardNumber);
                var receiverBank = await _dbContext.BankUserCard.Include(buc => buc.bank).FirstOrDefaultAsync(buc => buc.cardId == receiverCard.cardNumber);

                if (senderBank == null || receiverBank == null)
                {
                    return BadRequest("Bank information not found for one or both cards");
                }

                bool sameBank = senderBank.bankId == receiverBank.bankId;
                float transactionFee = 0.0f;
                if (!sameBank)
                {
                    transactionFee = request.amount * 0.03f;
                    var bank = senderBank.bank;
                    bank.totalCapital += transactionFee;
                }


                float convertedAmount = 0;
                var _currencyConverter = new CurrencyConverter();
                if (senderCard.currency.currencyCode != receiverCard.currency.currencyCode)
                {
                    convertedAmount = _currencyConverter.ConvertAmount(request.amount, senderCard.currency.currencyCode, receiverCard.currency.currencyCode);
                }
                senderCard.amount -= request.amount + transactionFee;
                receiverCard.amount += convertedAmount;

                transactionRecord = new Transaction
                {
                    transactionDate = DateTime.UtcNow,
                    amount = request.amount,
                    type = request.type,
                    status = "Pending",
                    senderCardId = request.senderCardId,
                    recieverCardId = request.recieverCardId
                };

                _dbContext.Transaction.Add(transactionRecord);

                var sendingUser = await _dbContext.BankUserCard.FirstOrDefaultAsync(buc => buc.cardId == senderCard.cardNumber);
                var user = await _dbContext.User.FirstOrDefaultAsync(u => u.id == sendingUser.userId);
                var senderConnectionString = _cache.Get<string>($"ConnectionId_{user.id}");


                if (user != null)
                {
                    await _loyaltyService.TrackActivity(user, senderCard, senderConnectionString);
                }

                await _dbContext.SaveChangesAsync();

                await Task.Delay(TimeSpan.FromSeconds(3));
                transactionRecord.status = "Completed";
                await _dbContext.SaveChangesAsync();
                transaction.Commit();

                var bankUserCard = await _dbContext.BankUserCard.FirstOrDefaultAsync(buc => buc.cardId == receiverCard.cardNumber);
                var receiverUser = await _dbContext.User.FirstOrDefaultAsync(u => u.id == bankUserCard.userId);

                var connectionId = _cache.Get<string>($"ConnectionId_{receiverUser.id}");

                if (!string.IsNullOrEmpty(connectionId))
                {
                    await _hubContext.Clients.Client(connectionId).SendAsync("message", "You received money: " + transactionRecord.amount);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing the transaction");
            }


            return Ok(new TransactionExecuteDetailVM
            {
                transactionId = transactionRecord?.transactionId ?? 0
            });

        }

        [HttpPost("deposit-withdrawal")]
        public async Task<ActionResult<TransactionExecuteDetailVM>> DepositOrWithdraw([FromBody] DepositWithdrawalVM request)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            Transaction transactionRecord = null;
            try
            {
                var receiverCard = await _dbContext.Card.Include(c => c.cardType).FirstOrDefaultAsync(c => c.cardNumber == request.recieverCardId);

                if (receiverCard == null)
                {
                    return BadRequest(new { message = "Invalid card number" });
                }

                receiverCard.amount += request.amount;

                if (receiverCard.amount < 0)
                {
                    return BadRequest(new { message = "Insufficient funds" });
                }

                transactionRecord = new Transaction
                {
                    transactionDate = DateTime.UtcNow,
                    amount = Math.Abs(request.amount),
                    type = request.type,
                    status = "Pending",
                    recieverCardId = request.recieverCardId
                };

                _dbContext.Transaction.Add(transactionRecord);
                await _dbContext.SaveChangesAsync();

                await Task.Delay(TimeSpan.FromSeconds(3));
                transactionRecord.status = "Completed";
                await _dbContext.SaveChangesAsync();
                transaction.Commit();

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }

            return Ok(new TransactionExecuteDetailVM
            {
                transactionId = transactionRecord?.transactionId ?? 0
            });

        }
    }
}
