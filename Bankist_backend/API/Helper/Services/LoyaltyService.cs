using API.Data;
using API.Data.Models;
using API.Endpoints.AuthEndpoints.Login;
using API.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace API.Helper.Services
{
    public class LoyaltyService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHubContext<SignalRHub> _hubContext;

        public LoyaltyService(ApplicationDbContext dbContext, IHubContext<SignalRHub> hubContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }

        public async Task TrackActivity(User user, Card senderCard, string connectionId)
        {
            var userActivity = await _dbContext.UserActivity.FirstOrDefaultAsync(u => u.id == user.id);

            if (userActivity == null)
            {
                throw new Exception("Missing user activity for this user.");
            }

            userActivity.transactionsCount += 1;
            
            if (userActivity.transactionsCount >= 5)
            {
                userActivity.accountStatus = "SILVER";
                if (userActivity.awardsReceived == 0)
                {
                    _dbContext.Transaction.Add(new Transaction
                    {
                        transactionDate = DateTime.Now,
                        amount = 50,
                        type = "Silver status award",
                        status = "Completed",
                        senderCard = null,
                        recieverCard = senderCard
                    });
                    userActivity.awardsReceived++;
                    if (!string.IsNullOrEmpty(connectionId))
                    {
                        await _hubContext.Clients.Client(connectionId).SendAsync("message", "Congratulations on reaching silver account status. Enjoy 50" + senderCard.currency.symbol + ".");
                    }
                }
            }

            if (userActivity.transactionsCount >= 10)
            {
                userActivity.accountStatus = "GOLD";
                if (userActivity.awardsReceived == 1)
                {
                    _dbContext.Transaction.Add(new Transaction
                    {
                        transactionDate = DateTime.Now,
                        amount = 100,
                        type = "Gold status award",
                        status = "Completed",
                        senderCard = null,
                        recieverCard = senderCard
                    });
                    userActivity.awardsReceived++;
                    if (!string.IsNullOrEmpty(connectionId))
                    {
                        await _hubContext.Clients.Client(connectionId).SendAsync("message", "Congratulations on reaching silver account status. Enjoy 100" + senderCard.currency.symbol + ".");
                    }
                }
            }

            if (userActivity.transactionsCount >= 15)
            {
                userActivity.accountStatus = "PLATINUM";
                if (userActivity.awardsReceived >= 2 &&  userActivity.transactionsCount % 15 == 0)
                {
                    _dbContext.Transaction.Add(new Transaction
                    {
                        transactionDate = DateTime.Now,
                        amount = 150,
                        type = "Platinum status award",
                        status = "Completed",
                        senderCard = null,
                        recieverCard = senderCard
                    });
                    userActivity.awardsReceived++;
                    if (!string.IsNullOrEmpty(connectionId))
                    {
                        await _hubContext.Clients.Client(connectionId).SendAsync("message", "Wow! You just keep on going! Enjoy 150" + senderCard.currency.symbol + ".");
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }

    }
}
