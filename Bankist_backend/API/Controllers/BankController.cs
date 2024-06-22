using API.Data;
using API.Data.Models;
using API.Endpoints.BankEndpoints.GetActiveBanks;
using API.Endpoints.BankEndpoints.GetAll;
using API.Endpoints.BankEndpoints.GetUnactiveBanks;
using API.Helper.Auth;
using API.Helper.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class Success
    {
        public string message { get; set; }
    }
    [MyAuthorization]
    [ApiController]
    [Route("[controller]")]
    public class BankController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BankController(ApplicationDbContext dbContext, MyAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet("get")]
        public async Task<ActionResult<BankGetAllVM>> GetBanks()
        {
            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }

            var banks = await _dbContext.Bank
                .OrderByDescending(x => x.id)
                .Select(x => new BankGetAllVMBank()
                {
                    bankId = x.id,
                    bankName = x.username,
                    password = x.password,
                    totalCapital = x.totalCapital,
                    numberOfUsers = x.numberOfUsers
                })
                .ToListAsync();

            var response = new BankGetAllVM
            {
                Banks = banks
            };

            return Ok(response);
        }

        [HttpGet("active-banks")]
        public async Task<ActionResult<GetActiveBanksVM>> GetActiveBanks()
        {
            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }
            Account account = _authService.GetAuthInfo().account!;
            if (!(account.isUser))
            {
                throw new Exception("Access denied");
            }
            string? authToken = _httpContextAccessor.HttpContext!.Request.Headers["Token"];
            var databaseToken = await _dbContext.AutentificationToken.FirstOrDefaultAsync(token => token.value == authToken);

            if (databaseToken != null)
            {
                var user = databaseToken.account.User;
                var bankUserCards = await _dbContext.BankUserCard.Where(item => item.user == user).ToListAsync();

                var banks = new GetActiveBanksVM();

                foreach (var bankUserCard in bankUserCards)
                {
                    var bank = await _dbContext.Bank.FirstOrDefaultAsync(b => b.id == bankUserCard.bankId);

                    if (bank != null)
                    {
                        banks.Banks.Add(new ActiveBankVM { Name = bank.username });
                    }
                }

                return Ok(banks);
            }

            return StatusCode(503);
        }


        [HttpGet("get-bank-users")]
        public async Task<ActionResult<BankGetUsersVM>> GetBankUsers([FromQuery] BankUserVM request)
        {
            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }
            Account account = _authService.GetAuthInfo().account!;
            if (!(account.isBank))
            {
                throw new Exception("Access denied");
            }
            var bankUsers = await _dbContext.BankUserCard
                   .Include(buc => buc.user)
                   .Include(buc => buc.card)
                   .Where(buc => buc.bankId == request.BankId)
                   .ToListAsync();

            var response = bankUsers.Select(buc => new BankGetUsersVMDetails
            {
                Id = buc.user.id,
                FirstName = buc.user.firstName,
                LastName = buc.user.lastName,
                Email = buc.user.email,
                Phone = buc.user.phone,
                BirthDate = buc.user.birthDate,
                RegistrationDate = buc.user.registrationDate,
                CardNumber = buc.card.cardNumber,
                Amount = buc.card.amount,
                ExpirationDate = buc.card.expirationDate
            }).ToList();

            return Ok(new BankGetUsersVM
            {
                Banks = response
            });
        }

        [HttpGet("unactive-banks")]
        public async Task<ActionResult<GetUnactiveBanksVM>> GetUnactiveBanks()
        {
            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }
            Account account = _authService.GetAuthInfo().account!;
            if (!(account.isUser))
            {
                throw new Exception("Access denied");
            }
            string? authToken = _httpContextAccessor.HttpContext!.Request.Headers["Token"];
            var databaseToken = _dbContext.AutentificationToken.FirstOrDefault(token => token.value == authToken);

            if (databaseToken != null)
            {
                var user = databaseToken.account.User;
                var bankUserCard = await _dbContext.BankUserCard.Where(item => item.user == user).ToListAsync();
                var banks = await _dbContext.Bank.ToListAsync();
                var unactiveBanks = new GetUnactiveBanksVM();

                foreach (var bank in banks)
                {
                    bool contains = bankUserCard.Any(buc => buc.bankId == bank.id);
                    if (!contains)
                    {
                        unactiveBanks.Banks.Add(new UnactiveBankVM() { Name = bank.username, NumberOfUsers = bank.numberOfUsers });
                    }
                }

                return Ok(unactiveBanks);
            }

            return StatusCode(503);
        }

        [HttpPost("new-account")]
        public async Task<ActionResult<Success>> OpenNewBankAccount([FromBody] BankAccountVM request)
        {
            if (!_authService.IsLogin())
            {
                return Unauthorized("Unauthorized");
            }
            Account account = _authService.GetAuthInfo().account!;
            if (!(account.isUser))
            {
                throw new Exception("Access denied");
            }
            string? authToken = _httpContextAccessor.HttpContext!.Request.Headers["Token"];
            var databaseToken = _dbContext.AutentificationToken.FirstOrDefault(token => token.value == authToken);

            if (databaseToken != null)
            {
                if (request.amount < 100)
                {
                    return BadRequest("Invalid amount");
                }
                if (request.amount > 10000)
                {
                    return BadRequest("Amount too high");
                }

                var user = databaseToken.account.User;
                var maxCardNumber = _dbContext.Card.Max(c => (int?)c.cardNumber) ?? 0;
                var lastPin = _dbContext.Card.OrderByDescending(c => c.pin).FirstOrDefault()?.pin ?? 0;

                var card = new Card
                {
                    cardNumber = maxCardNumber + 1,
                    issueDate = DateTime.Now,
                    expirationDate = DateTime.Now.AddYears(1),
                    amount = request.amount,
                    cardType = _dbContext.CardType.FirstOrDefault(type => type.CardTypeId == request.type),
                    currency = _dbContext.Currency.FirstOrDefault(currency => currency.currencyCode == request.currency),
                    pin = lastPin + 1,
                    transactionLimit=100,
                    atmLimit=100,
                    negativeLimit=0
                };
                _dbContext.Card.Add(card);

                var bank = _dbContext.Bank.FirstOrDefault(bank => bank.username == request.name);

                var bankUserCard = new BanksUsersCards
                {
                    user = user,
                    bank = bank,
                    card = card,
                    accountIssueDate = DateTime.Now,
                };
                _dbContext.Add(bankUserCard);

                bank.numberOfUsers++;

                await _dbContext.SaveChangesAsync();

                var successResponse = new Success();
                successResponse.message = "Account successfully opened";

                return Ok(successResponse);
            }

            return StatusCode(503);
        }
        [HttpDelete("delete-account")]
        public ActionResult CloseUserAccount(int userId, int bankId, string reason)
        {

            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }
            Account account = _authService.GetAuthInfo().account!;
            if (!(account.isBank))
            {
                throw new Exception("Access denied");
            }
            var user = _dbContext.User.FirstOrDefault(x => x.id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var userCard = _dbContext.BankUserCard.FirstOrDefault(x => x.userId == userId && x.bankId == bankId);

            if (userCard == null)
            {
                return NotFound("User's card in this bank not found.");
            }

            try
            {
                var deletedUser = new DeletedUser
                {
                    firstName = user.firstName,
                    lastName = user.lastName,
                    email = user.email,
                    phone = user.phone,
                    birthDate = user.birthDate,
                    registrationDate = user.registrationDate,
                    reason = reason,
                    password = user.password,
                    username = user.username,
                    deletionDate = DateTime.Now
                };
                _dbContext.DeletedUser.Add(deletedUser);

                _dbContext.BankUserCard.Remove(userCard);

                _dbContext.SaveChanges();

                return Ok("User's account has been successfully closed in the selected bank.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while closing user's account: {ex.Message}");
            }

        }


        [HttpPut("block-card")]
        public ActionResult BlockCard(int userId, int bankId)
        {

            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }
            Account account = _authService.GetAuthInfo().account!;
            if (!(account.isBank))
            {
                throw new Exception("Access denied");
            }
            var user = _dbContext.User.FirstOrDefault(x => x.id == userId);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var userCard = _dbContext.BankUserCard.FirstOrDefault(x => x.userId == userId && x.bankId == bankId);

            if (userCard == null)
            {
                return NotFound("User's card in this bank not found.");
            }
            userCard.isBlock = true;
            _dbContext.SaveChanges();
            return Ok("User's card has been successfully blocked.");
        }

        [HttpGet("get-blockedCards")]
        public ActionResult GetBlockedCard([FromQuery] int bankId)
        {
            var cards = _dbContext.BankUserCard.Where(buc => buc.isBlock == true).Include(x => x.card).Where(buc => buc.bankId == bankId);
            return Ok(cards);
        }
    }
}


