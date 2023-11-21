using API.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Data
{
    [ApiController]
    [Route("[controller]")]
    public class BankController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public BankController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public List<Bank> GetBanks()
        {
            var rezultat = _dbContext.Bank;
            return rezultat.ToList();
        }
    }
}
