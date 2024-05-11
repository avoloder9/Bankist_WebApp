using API.Data;
using API.Data.Models;
using API.Endpoints.AuthEndpoints.Login;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace API.Helper.Services
{
    public class LoyaltyService
    {
        private readonly ApplicationDbContext _dbContext;
        public LoyaltyService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task TrackActivity(User user)
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
            }

            if (userActivity.transactionsCount >= 10)
            {
                userActivity.accountStatus = "GOLD";
            }

            if (userActivity.transactionsCount >= 15)
            {
                userActivity.accountStatus = "PLATINUM";
            }

            await _dbContext.SaveChangesAsync();
        }

    }
}
