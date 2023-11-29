using API.Data;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints.UserEndpoints.CheckExists
{
    public interface IUserService
    {
        bool UserExists(string username, string email);

        public class UserService : IUserService
        {
            private readonly ApplicationDbContext _dbContext;
            public UserService(ApplicationDbContext dbContext)
            {
                _dbContext = dbContext;
            }
            public bool UserExists(string username, string email)
            {
                return _dbContext.User.Any(u => u.userName == username || u.email == email);
            }
        }
    }
}