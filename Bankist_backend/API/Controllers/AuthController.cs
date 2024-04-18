using API.Data;
using API.Data.Models;
using API.Helper.Services;
using API.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Endpoints.AuthEndpoints.Login;

namespace API.Controllers
{
    [Route("Auth")]
    public class AuthController: ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;

        public AuthController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<ActionResult<MyAuthInfo>> Login([FromBody] AuthLoginVM request)
        {
            Account loggedInAccount = await _dbContext.Account.FirstOrDefaultAsync(u => u.username == request.username);

            if (loggedInAccount == null)
            {                
                return NotFound("Username not found.");
            }
                       
            if (loggedInAccount.password != request.password)
            {                
                return Unauthorized("Incorrect password.");
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
            await _dbContext.SaveChangesAsync();

            return Ok(new MyAuthInfo(newToken));
        }

        [HttpPost("get")]
        public async Task<ActionResult<MyAuthInfo>> GetAuthInfo([FromBody] NoRequest request)
        {
            AutentificationToken? autentificationToken = _authService.GetAuthInfo().autentificationToken;

            if (autentificationToken == null)
            {
                return NotFound();
            }

            return Ok(new MyAuthInfo(autentificationToken));
        }
        [HttpPost("logout")]
        public async Task<ActionResult<NoResponse>> Logout([FromBody] NoRequest request)
        {
            AutentificationToken? autentificationToken = _authService.GetAuthInfo().autentificationToken;

            if (autentificationToken == null)
            {
                return NotFound();
            }

            _dbContext.Remove(autentificationToken);
            await _dbContext.SaveChangesAsync();

            return Ok(new NoResponse());
        }


    }
}
