using API.Data;
using API.Data.Models;
using API.Endpoints.TransactionEndpoints.Execute;
using API.Helper.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using API.Helper.Auth;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
  
    public class TransactionController : ControllerBase
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionController(ApplicationDbContext dbContext, MyAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
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

                var transactions = _dbContext.Transaction
              .Where(t => (_dbContext.BankUserCard
                              .Where(buc => buc.userId == user.id && buc.bank.username == bankName)
                              .Select(buc => buc.cardId)
                              .Contains(t.senderCardId) ||
                           _dbContext.BankUserCard
                              .Where(buc => buc.userId == user.id && buc.bank.username == bankName)
                              .Select(buc => buc.cardId)
                              .Contains(t.recieverCardId))
                          )

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
                var transactions = _dbContext.Transaction
                    .Where(t => _dbContext.BankUserCard
                                    .Where(buc => buc.bankId == bankId)
                                    .Select(buc => buc.cardId)
                                    .Contains(t.senderCardId) ||
                                _dbContext.BankUserCard
                                    .Where(buc => buc.bankId == bankId)
                                    .Select(buc => buc.cardId)
                                    .Contains(t.recieverCardId))
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
                var senderCard = await _dbContext.Card.Include(c => c.cardType).FirstOrDefaultAsync(c => c.cardNumber == request.senderCardId);
                var receiverCard = await _dbContext.Card.Include(c => c.cardType).FirstOrDefaultAsync(c => c.cardNumber == request.recieverCardId);

                if (senderCard == null || receiverCard == null)
                {
                    return BadRequest("Invalid card number");
                }

                if (senderCard.amount < request.amount)
                {
                    return StatusCode(401, "Insufficient funds");
                }

                senderCard.amount -= request.amount;
                receiverCard.amount += request.amount;

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
                
                await _dbContext.SaveChangesAsync();

                await Task.Delay(TimeSpan.FromSeconds(3));
                transactionRecord.status = "Completed";
                await _dbContext.SaveChangesAsync();
                transaction.Commit();
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


    }
}
