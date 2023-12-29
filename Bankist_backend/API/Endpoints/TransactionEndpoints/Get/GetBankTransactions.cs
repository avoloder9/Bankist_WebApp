using API.Data;
using API.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.TransactionEndpoints.Get
{
    [ApiController]
    [Route("[controller]")]
    public class GetBankTransactions : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public GetBankTransactions(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet]
        public ActionResult<List<Transaction>> GetBankTransaction(int bankId)
        {
            try
            {
                var transactions = _dbContext.Transaction
                    .Where(t => _dbContext.BankUserCard
                                    .Where(buc => buc.bankId == bankId)
                                    .Select(buc => buc.cardId)
                                    .Contains(t.senderCardId) ||
                                _dbContext.BankUserCard
                                    .Where(buc => buc.bankId == bankId)
                                    .Select(buc => buc.cardId)
                                    .Contains(t.recieverCardId))
                    .OrderByDescending(t => t.transactionDate)
                    .ToList();

                if (!transactions.Any())
                {
                    return NotFound($"No transactions found for bank with ID {bankId}.");
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
