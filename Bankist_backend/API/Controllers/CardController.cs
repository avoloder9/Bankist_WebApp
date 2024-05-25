using API.Data;
using API.Data.Models;
using API.Endpoints.AuthEndpoints.Login;
using API.Helper.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class CardAuthResponse
    {
        public MyAuthInfo Token { get; set; }
        public Card CardInfo { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MyAuthService _authService;

        public CardController(ApplicationDbContext dbContext, MyAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _authService = authService;
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

                var banksUsersCard = _dbContext.BankUserCard.Include(x => x.user).Include(x => x.card).Include(x => x.bank).Include(x => x.card.cardType).Include(x => x.card.currency)
        .FirstOrDefault(buc => buc.bank.username == bankName && buc.userId == userId);

                var status = _dbContext.UserActivity.FirstOrDefault(activity => activity.id == userId);

                if (banksUsersCard != null)
                {
                    {
                        string userName = $"{banksUsersCard.user.firstName} {banksUsersCard.user.lastName}";

                        if (banksUsersCard.cardId != null)
                        {
                            return Ok(new
                            {
                                Id = banksUsersCard.id,
                                //BankId = banksUsersCard.bank.username,
                                Bank = banksUsersCard.bank.username,
                                User = userName.ToString(),
                                UserId = userId,
                                CardNumber = banksUsersCard.card.cardNumber,
                                ExpirationDate = banksUsersCard.card.expirationDate,
                                IssueDate = banksUsersCard.card.issueDate,
                                Amount = banksUsersCard.card.amount,
                                CardTypeId = banksUsersCard.card.cardType.CardTypeId,
                                CurrencyId = banksUsersCard.card.currency.currencyCode,
                                status = status.accountStatus,
                            });
                        }
                        else
                        {
                            return NotFound("Card not found.");
                        }
                    }
                }
                else
                {
                    return NotFound("User not found.");
                }
            }
            else
            {
                return NotFound("User's card was not found at the specified bank. ");
            }

        }


        [HttpGet("check-card")]
        public ActionResult CheckCardNumber([FromQuery] string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return BadRequest(new { message = "Card number is required." });
            }

            if (!long.TryParse(cardNumber, out long cardNumberAsLong))
            {
                return BadRequest(new { message = "Invalid card number." });
            }

            var card = _dbContext.Card.FirstOrDefault(card => card.cardNumber == cardNumberAsLong);

            if (card == null)
            {
                return NotFound(new { message = "Card not found." });
            }

            return Ok(new { message = "Card exists" });
        }

        [HttpPost("authenticate-card-owner")]
        public async Task<ActionResult<CardAuthResponse>> AuthenticateCardOwner(CardPinVM cardPin)
        {
            if (cardPin == null)
            {
                return BadRequest();
            }

            var card = await _dbContext.Card.FirstOrDefaultAsync(c => c.cardNumber == cardPin.cardNumber);

            if (card == null)
            {
                return NotFound();
            }

            if (card.pin != cardPin.pin)
            {
                return BadRequest(new { message = "Incorrect pin" });
            }

            var bankUserCard = await _dbContext.BankUserCard.FirstOrDefaultAsync(buc => buc.cardId == card.cardNumber);
            var account = await _dbContext.Account.FirstOrDefaultAsync(acc => acc.id == bankUserCard.userId);

            AuthLoginVM loginRequest = new AuthLoginVM
            {
                username = account.username,
                password = account.password
            };

            try
            {
                var authInfo = await _authService.Login(loginRequest);

                var response = new CardAuthResponse
                {
                    Token = authInfo,
                    CardInfo = card
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error during authentication: {ex.Message}");
            }
        }
        [HttpGet("card-balance")]
        public ActionResult CardBalance([FromQuery] string cardNumber)
        {
            string? authToken = _httpContextAccessor.HttpContext!.Request.Headers["Token"];
            var databaseToken = _dbContext.AutentificationToken.FirstOrDefault(token => token.value == authToken);

            if (databaseToken == null) return Unauthorized();

            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return BadRequest(new { message = "Card number is required." });
            }

            if (!long.TryParse(cardNumber, out long cardNumberAsLong))
            {
                return BadRequest(new { message = "Invalid card number." });
            }

            var card = _dbContext.Card.FirstOrDefault(card => card.cardNumber == cardNumberAsLong);

            if (card == null)
            {
                return NotFound(new { message = "Card not found." });
            }

            return Ok(new { amount = card.amount });
        }
    }
}
