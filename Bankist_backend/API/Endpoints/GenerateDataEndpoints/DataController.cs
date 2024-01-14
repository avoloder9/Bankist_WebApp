using API.Data;
using API.Data.Models;
using API.Helper;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.GenerateDataEndpoints
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DataController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public DataController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult Count()
        {
            Dictionary<string, int> data = new Dictionary<string, int>();
            data.Add("user", _dbContext.User.Count());
            data.Add("bank", _dbContext.Bank.Count());
            data.Add("curreny", _dbContext.Currency.Count());
            data.Add("cardTypes", _dbContext.CardType.Count());
            data.Add("Cards", _dbContext.Card.Count());
            data.Add("BankUserCard", _dbContext.BankUserCard.Count());
            return Ok(data);
        }
        [HttpPost]
        public ActionResult Generate()
        {
            var users = new List<User>();
            var banks = new List<Bank>();
            var currencies = new List<Currency>();
            var cardTypes = new List<CardType>();
            var cards = new List<Card>();
            var bankUserCard=new List<BanksUsersCards>();

            banks.Add(new Bank { username = "Unicredit", password = TokenGenerator.GeneratePassword(), totalCapital = 0, numberOfUsers = 0 });
            banks.Add(new Bank { username = "Raiffeisen", password = TokenGenerator.GeneratePassword(), totalCapital = 0, numberOfUsers = 0 });
            banks.Add(new Bank { username = "Ziraat", password = TokenGenerator.GeneratePassword(), totalCapital = 0, numberOfUsers = 0 });
            banks.Add(new Bank { username = "ASA", password = TokenGenerator.GeneratePassword(), totalCapital = 0, numberOfUsers = 0 });
            banks.Add(new Bank { username = "ProCredit", password = TokenGenerator.GeneratePassword(), totalCapital = 0, numberOfUsers = 0 });
            banks.Add(new Bank { username = "Sparkasse", password = TokenGenerator.GeneratePassword(), totalCapital = 0, numberOfUsers = 0 });

            Random rnd = new Random();

            for (int i = 0; i < 100; i++)
            {
                users.Add(new User
                {
                    firstName = TokenGenerator.GenerateName(5),
                    lastName = TokenGenerator.GenerateName(5),
                    email = TokenGenerator.GenerateRandomEmail(),
                    phone = TokenGenerator.GenerateRandomPhoneNumber(),
                    birthDate = TokenGenerator.GenerateRandomBirthDate(),
                    registrationDate = DateTime.Now,
                    username = TokenGenerator.GenerateName(5),
                    password = TokenGenerator.GeneratePassword()
                });
            }

            currencies.Add(new Currency { currencyCode="BAM", currencyName="Marks", symbol="BAM", exchangeRate=1 });
            currencies.Add(new Currency { currencyCode = "EUR", currencyName = "Euro", symbol = "€", exchangeRate = 1.94f });
            currencies.Add(new Currency { currencyCode="USD", currencyName="Dollar", symbol="$", exchangeRate=1.71f });

            cardTypes.Add(new CardType { CardTypeId = "DEBIT", fees = 2, maxLimit = 100000 });
            cardTypes.Add(new CardType { CardTypeId = "CREDIT", fees = 2, maxLimit = 100000 });

            cards.Add(new Card { cardNumber = 111111, issueDate = new DateTime(2023, 1, 1), expirationDate = new DateTime(2024, 1, 1), amount = 1000.45f, cardType = cardTypes[0], currency = currencies[0]});
            cards.Add(new Card { cardNumber = 111112, issueDate = new DateTime(2023, 1, 1), expirationDate = new DateTime(2024, 1, 1), amount = 1000.45f, cardType = cardTypes[1], currency = currencies[1]});

            bankUserCard.Add(new BanksUsersCards { user = users[0], bank = banks[0], card = cards[0], accountIssueDate = cards[0].issueDate });
            bankUserCard.Add(new BanksUsersCards { user = users[0], bank = banks[1], card = cards[1], accountIssueDate = cards[1].issueDate });
            bankUserCard.Add(new BanksUsersCards { user = users[1], bank = banks[1], card = cards[1], accountIssueDate = cards[1].issueDate });


            _dbContext.AddRange(users);
            _dbContext.AddRange(banks);
            _dbContext.AddRange(currencies);
            _dbContext.AddRange(cardTypes);
            _dbContext.AddRange(cards);
            _dbContext.AddRange(bankUserCard);

            _dbContext.SaveChanges();
            return Count();
        }
    }
}
