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
            return Ok(data);
        }
        [HttpPost]
        public ActionResult Generate()
        {
            var users = new List<User>();
            var banks = new List<Bank>();

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
            _dbContext.AddRange(users);
            _dbContext.AddRange(banks);

            _dbContext.SaveChanges();
            return Count();
        }
    }
}
