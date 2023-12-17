using API.Data;
using API.Data.Models;
using API.Helper;
using API.Helper.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Endpoints.UserEndpoints;
namespace API.Endpoints.AuthEndpoints.Login
{
    [Route("auth")]
    public class AuthLoginEndpoint : MyBaseEndpoint<AuthLoginRequest, MyAuthInfo>
    {
        private readonly ApplicationDbContext _dbContext;
        public AuthLoginEndpoint(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public override async Task<MyAuthInfo> Procces([FromBody]AuthLoginRequest request, CancellationToken cancellationToken)
        {
          if (string.IsNullOrEmpty(request.username) || string.IsNullOrEmpty(request.password))
            {
                Response.StatusCode = 401;
                return new MyAuthInfo(null);
            }
            
          
            Account? loggedInAccount = await _dbContext.Account.FirstOrDefaultAsync(u => u.username == request.username && u.password == request.password, cancellationToken);

            if (loggedInAccount == null)
            {
                Response.StatusCode = 404;
                return new MyAuthInfo(null);
            }

            string randomString = TokenGenerator.Generate(10);
            var newToken = new AutentificationToken()
            {
                value = randomString,
                ipAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                account = loggedInAccount,
                autentificationTimestamp = DateTime.Now
            };
            _dbContext.Add(newToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            Response.StatusCode = 200;

            return new MyAuthInfo(newToken);
        }

    }
}
