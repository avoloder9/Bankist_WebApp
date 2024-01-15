using API.Data;
using API.Data.Models;
using API.Endpoints.BankEndpoints.GetActiveBanks;
using API.Helper.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.TransactionEndpoints.Get
{
    [ApiController]
    [Route("[controller]")]
    public class GetUserTransactions : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetUserTransactions(ApplicationDbContext dbContext, MyAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
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

                foreach (var transaction in transactions)
                {
                    if (_dbContext.BankUserCard.Any(buc => buc.cardId == transaction.senderCardId))
                    {
                      //treba dodati -
                        transaction.amount = Math.Abs(transaction.amount);
                    }
                    else if (_dbContext.BankUserCard.Any(buc => buc.cardId == transaction.recieverCardId))
                    {
                        
                        transaction.amount = Math.Abs(transaction.amount);
                    }
                }
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
