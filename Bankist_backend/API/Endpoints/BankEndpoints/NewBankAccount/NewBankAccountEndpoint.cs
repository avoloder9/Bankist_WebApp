using API.Data;
using API.Data.Models;
using API.Helper;
using API.Helper.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints.BankEndpoints.NewBankAccount
{
    public class Success { 
        public string message {  get; set; }
    }
    [ApiController]
    [Route("[controller]")]
    public class NewBankAccountEndpoint : MyBaseEndpoint<NewBankAccountRequest, ActionResult<Success>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public NewBankAccountEndpoint(ApplicationDbContext dbContext, MyAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpPost]
        public override async Task<ActionResult<Success>> Procces([FromBody] NewBankAccountRequest request, CancellationToken cancellationToken)
        {
            if (!_authService.IsLogin())
            {
                Response.StatusCode = 401;
                return BadRequest("Unauthorized");
            }

            string? authToken = _httpContextAccessor.HttpContext!.Request.Headers["Token"];
            var databaseToken = _dbContext.AutentificationToken.Where(token => token.value == authToken).ToList();
            var user = new User();
            if (databaseToken[0] != null)
            {
                if(request.amount < 100)
                {
                    return BadRequest("Invalid amount");
                }
                if(request.amount > 10000)
                {
                    return BadRequest("Amount too high");
                }
                user = databaseToken[0].account.User;
                var maxCardNumber = _dbContext.Card.Max(c => (int?)c.cardNumber) ?? 0;
               
                var card = new Card { 
                    cardNumber = maxCardNumber + 1, 
                    issueDate = DateTime.Now, 
                    expirationDate = DateTime.Now.AddYears(1),
                    amount = request.amount, 
                    cardType = _dbContext.CardType.FirstOrDefault(type => type.CardTypeId == request.type),
                    currency =  _dbContext.Currency.FirstOrDefault(currency => currency.currencyCode == request.currency)
                };
                _dbContext.Card.Add(card);
                var bankUserCard = new BanksUsersCards
                {
                    user = user,
                    bank = _dbContext.Bank.FirstOrDefault(bank => bank.username == request.name),
                    card = card,
                    accountIssueDate = DateTime.Now,
                };
                _dbContext.Add(bankUserCard);
                _dbContext.SaveChangesAsync();
                Response.StatusCode = 200;
                var x = new Success();
                x.message = "Account successfully opened";
                return x;
            }

            Response.StatusCode = 503;
            return BadRequest("Internal server error");

        }
    }
}
