using API.Data;
using API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CardController(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("card-info")]
        public ActionResult GetCardInfo([FromQuery] string bankName)
        {
            string? authToken = _httpContextAccessor.HttpContext!.Request.Headers["Token"];
            var databaseToken = _dbContext.AutentificationToken.FirstOrDefault(token => token.value == authToken);

            if (databaseToken != null)
            {
                var userId = databaseToken.accountId;

                var banksUsersCard = _dbContext.BankUserCard.Include(x=>x.user).Include(x => x.card).Include(x => x.bank).Include(x => x.card.cardType).Include(x => x.card.currency)
        .FirstOrDefault(buc => buc.bank.username == bankName && buc.userId == userId);

                if (banksUsersCard != null)
                {
                    {
                        string userName = $"{banksUsersCard.user.firstName} {banksUsersCard.user.lastName}";

                        if (banksUsersCard.cardId != null)
                        {
                            return Ok(new
                            {
                                Id = banksUsersCard.id,
                                BankId = banksUsersCard.bank.username,
                                UserId = userName,
                                CardNumber = banksUsersCard.card.cardNumber,
                                ExpirationDate = banksUsersCard.card.expirationDate,
                                IssueDate = banksUsersCard.card.issueDate,
                                Amount = banksUsersCard.card.amount,
                                CardTypeId = banksUsersCard.card.cardType.CardTypeId,
                                CurrencyId = banksUsersCard.card.currency.currencyCode
                            });
                        }
                        else
                        {
                            return NotFound("Kartica nije pronađena.");
                        }
                    }
                }
                else
                {
                    return NotFound("Korisnik nije pronađen.");
                }
            }
            else
            {
                return NotFound("Kartica korisnika nije pronađena u određenoj banci.");
            }

        }
    }
}
