using API.Data;
using API.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.TransactionEndpoints.Get
{
    [ApiController]
    [Route("[controller]")]
    public class GetUserTransactions : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public GetUserTransactions(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public ActionResult<List<Transaction>> GetUserTransaction([FromQuery] int userId, int bankId/*,int cardId*/)
        {
            try
            {
                /*   var transactions = _dbContext.Transaction
                .Where(t => (t.senderCardId == cardId || t.recieverCardId == cardId) &&
                            (_dbContext.BankUserCard
                                .Where(buc => buc.cardId == cardId && buc.userId == userId && buc.bankId == bankId)
                                .Any())
                            )
                */

                var userExistsInBank = _dbContext.BankUserCard
           .Any(buc => buc.userId == userId && buc.bankId == bankId);

                if (!userExistsInBank)
                {
                    return BadRequest($"User with ID {userId} does not have an account in the bank with ID {bankId}.");
                }

                var transactions = _dbContext.Transaction
              .Where(t => (_dbContext.BankUserCard
                              .Where(buc => buc.userId == userId && buc.bankId == bankId)
                              .Select(buc => buc.cardId)
                              .Contains(t.senderCardId) ||
                           _dbContext.BankUserCard
                              .Where(buc => buc.userId == userId && buc.bankId == bankId)
                              .Select(buc => buc.cardId)
                              .Contains(t.recieverCardId))
                          )

               .OrderByDescending(t => t.transactionDate)
               .ToList();

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
