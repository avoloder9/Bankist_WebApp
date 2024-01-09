using API.Data;
using API.Data.Models;
using API.Endpoints.TransactionEndpoints.Execute;
using API.Helper;
using API.Helper.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints.TransactionEndpoints.Execute
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionExecuteEndpoint : MyBaseEndpoint<TransactionExecuteRequest, TransactionExecuteResponse>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;
        public TransactionExecuteEndpoint(ApplicationDbContext dbContext, MyAuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }
        [HttpPost]
        public override async Task<TransactionExecuteResponse> Procces([FromBody] TransactionExecuteRequest request, CancellationToken cancellationToken)
        {

            var transaction = _dbContext.Database.BeginTransaction();
            Transaction transactionRecord = null;
            try
            {
                //  var currentUserId = _authService.GetAuthInfo().account?.id;


                var senderCard = await _dbContext.Card.Include(c => c.cardType).FirstOrDefaultAsync(c => c.cardNumber == request.senderCardId);
                var receiverCard = await _dbContext.Card.Include(c => c.cardType).FirstOrDefaultAsync(c => c.cardNumber == request.recieverCardId);

                if (senderCard == null || receiverCard == null)
                {
                    Response.StatusCode = 400;
                    throw new ArgumentNullException("Invalid card number");
                }

                if (senderCard.amount < request.amount)
                {
                    Response.StatusCode = 401;
                    throw new InvalidOperationException("Insufficient funds");
                }

                senderCard.amount -= request.amount;
                receiverCard.amount += request.amount;

                transactionRecord = new Transaction
                {
                    transactionDate = DateTime.UtcNow,
                    amount = request.amount,
                    type = request.type,
                    status = request.status,
                    senderCardId = request.senderCardId,
                    recieverCardId = request.recieverCardId,
                    //     transactionToken=Guid.NewGuid().ToString()                   
                };

                _dbContext.Transaction.Add(transactionRecord);

                await _dbContext.SaveChangesAsync(cancellationToken);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw ex;
            }
            return new TransactionExecuteResponse
            {
                transactionId = transactionRecord.transactionId,
                //        transactionToken = transactionRecord.transactionToken
            };

        }
    }
}
