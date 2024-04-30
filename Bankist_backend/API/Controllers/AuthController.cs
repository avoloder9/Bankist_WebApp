using API.Data;
using API.Data.Models;
using API.Helper.Services;
using API.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Endpoints.AuthEndpoints.Login;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;
using API.SignalR;
using API.ViewModels;
using Microsoft.Extensions.Caching.Memory;

namespace API.Controllers
{
    [Route("Auth")]
    public class AuthController: ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;
       private readonly IHubContext<SignalRHub> _hubContext;
        private readonly IMemoryCache _cache;
        public AuthController(ApplicationDbContext dbContext, IHubContext<SignalRHub> hubContext, MyAuthService authService, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
            _authService = authService;
            _cache = cache;
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

            _cache.Set($"ConnectionId_{loggedInAccount.id}", request.SignalRConnectionID);

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
        public async Task<ActionResult<NoResponse>> Logout([FromBody] AuthLogoutVM request)
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
