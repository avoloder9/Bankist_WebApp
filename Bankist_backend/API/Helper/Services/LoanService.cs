using API.Data.Models;
using API.Data;
using API.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Caching.Memory;

namespace API.Helper.Services
{
    public class LoanService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHubContext<SignalRHub> _hubContext;
        private readonly IMemoryCache _cache;


        public LoanService(IServiceScopeFactory scopeFactory, IHubContext<SignalRHub> hubContext, IMemoryCache cache)
        {
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
            _cache = cache;
        }

        public async Task RatePay(Loan loan, Card card, Bank bank)
        {
            for (int i = 0; i < loan.rateCount; i++)
            {
                await Task.Delay(TimeSpan.FromSeconds(20));
                using (var scope = _scopeFactory.CreateScope())
                {
                    var _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var transaction = new Transaction
                    {
                        transactionDate = DateTime.UtcNow,
                        amount = loan.rate,
                        type = "Rate",
                        status = "Completed",
                        senderCardId = null,
                        recieverCardId = card.cardNumber
                    };

                    _dbContext.Transaction.Add(transaction);

                    var loanToUpdate = await _dbContext.Loan.FirstOrDefaultAsync(l => l.loanId == loan.loanId);
                    if (loanToUpdate != null)
                    {
                        loanToUpdate.ratesPayed++;
                        loanToUpdate.totalAmountPayed += loan.rate;
                        loanToUpdate.remainingAmount -= loan.rate;
                        loanToUpdate.status = "ACTIVE";
                        if (i == loan.rateCount - 1)
                        {
                            loanToUpdate.status = "COMPLETED";
                        }
                    }


                    await _dbContext.SaveChangesAsync();

                    var buc = _dbContext.BankUserCard.FirstOrDefault(buc => buc.cardId == card.cardNumber);

                    if (buc != null)
                    {
                        var user = _dbContext.User.FirstOrDefault(u => u.id == buc.userId);
                        if (user != null)
                        {
                            var connectionId = _cache.Get<string>($"ConnectionId_{user.id}");
                            if (!string.IsNullOrEmpty(connectionId))
                            {
                                await _hubContext.Clients.Client(connectionId).SendAsync("rate", "Loan rate has been deducted from your account. Amount " + loanToUpdate?.rate + card.currency.currencyCode);
                            }
                        }
                    }

                }
            }
        }
    }
}
