using API.Data;
using API.Data.Models;
using API.Helper;
using Microsoft.AspNetCore.Mvc;
using System;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class GenerateDataController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public GenerateDataController(ApplicationDbContext dbContext)
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
            data.Add("Transaction", _dbContext.Transaction.Count());

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
            var bankUserCard = new List<BanksUsersCards>();
            var transactions = new List<Transaction>();

            banks.Add(new Bank { username = "Unicredit", password = TokenGenerator.GeneratePassword(), totalCapital = 0, numberOfUsers = 0 });
            banks.Add(new Bank { username = "Raiffeisen", password = TokenGenerator.GeneratePassword(), totalCapital = 0, numberOfUsers = 0 });
            banks.Add(new Bank { username = "Ziraat", password = TokenGenerator.GeneratePassword(), totalCapital = 0, numberOfUsers = 0 });
            banks.Add(new Bank { username = "ASA", password = TokenGenerator.GeneratePassword(), totalCapital = 0, numberOfUsers = 0 });
            banks.Add(new Bank { username = "ProCredit", password = TokenGenerator.GeneratePassword(), totalCapital = 0, numberOfUsers = 0 });
            banks.Add(new Bank { username = "Sparkasse", password = TokenGenerator.GeneratePassword(), totalCapital = 0, numberOfUsers = 0 });
            banks.Add(new Bank { username = "Addiko", password = TokenGenerator.GeneratePassword(), totalCapital = 0, numberOfUsers = 0 });
            banks.Add(new Bank { username = "BBI", password = TokenGenerator.GeneratePassword(), totalCapital = 0, numberOfUsers = 0 });
            banks.Add(new Bank { username = "Sanpaolo", password = TokenGenerator.GeneratePassword(), totalCapital = 0, numberOfUsers = 0 });

            users.Add(new User { firstName = "Adnan", lastName = "Voloder", email = "adnanvoloder@gmail.com", phone = "061312132", birthDate = TokenGenerator.GenerateRandomBirthDate(), registrationDate = DateTime.Now, username = "adnanv1", password = "Adnan123!" });
            users.Add(new User { firstName = "Faris", lastName = "Dizdarevic", email = "farisdiz@gmail.com", phone = "061341232", birthDate = TokenGenerator.GenerateRandomBirthDate(), registrationDate = DateTime.Now, username = "farisDiz1", password = "Faris123!" });

            Random random = new Random();

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

            currencies.Add(new Currency { currencyCode = "BAM", currencyName = "Marks", symbol = "BAM", exchangeRate = 1 });
            currencies.Add(new Currency { currencyCode = "EUR", currencyName = "Euro", symbol = "€", exchangeRate = 1.94f });
            currencies.Add(new Currency { currencyCode = "USD", currencyName = "Dollar", symbol = "$", exchangeRate = 1.71f });

            cardTypes.Add(new CardType { CardTypeId = "DEBIT", fees = 2, maxLimit = 100000 });
            cardTypes.Add(new CardType { CardTypeId = "CREDIT", fees = 2, maxLimit = 100000 });
            
            int startingCardNumber = 111111;
            for (int i = 0; i < 100; i++)
            {
                CardType randomCardType = cardTypes[random.Next(cardTypes.Count)];
                Currency randomCurrency = currencies[random.Next(currencies.Count)];

                int cardNumber = startingCardNumber + i;

                DateTime issueDate = DateTime.Now.AddDays(-random.Next(1, 365));
                DateTime expirationDate = issueDate.AddYears(1);

                float amount = (float)random.NextDouble() * 1000;

                var card = new Card
                {
                    cardNumber = cardNumber,
                    issueDate = issueDate,
                    expirationDate = expirationDate,
                    amount = amount,
                    cardType = randomCardType,
                    currency = randomCurrency
                };
                cards.Add(card);
            }

            foreach (var card in cards)
            {
                var randomUser = users[random.Next(users.Count)];
                var randomBank = banks[random.Next(banks.Count)];

                bankUserCard.Add(new BanksUsersCards { user = randomUser, bank = randomBank, card = card, accountIssueDate = card.issueDate });
            }

            for (int i = 0; i < 100; i++)
            {
                var senderCard = cards[random.Next(cards.Count)];
                var receiverCard = cards[random.Next(cards.Count)];

                while (senderCard.cardNumber == receiverCard.cardNumber)
                {
                    receiverCard = cards[random.Next(cards.Count)];
                }

                transactions.Add(new Transaction
                {
                    transactionDate = DateTime.Now,
                    amount = (float)random.NextDouble() * 1000,
                    type = TokenGenerator.GeneratePurpose(),
                    status = "Completed",
                    senderCard = senderCard,
                    recieverCard = receiverCard
                });
            }

            _dbContext.AddRange(users);
            _dbContext.AddRange(banks);
            _dbContext.AddRange(currencies);
            _dbContext.AddRange(cardTypes);
            _dbContext.AddRange(cards);
            _dbContext.AddRange(bankUserCard);
            _dbContext.AddRange(transactions);
            _dbContext.SaveChanges();
            return Count();
        }
    }
}
