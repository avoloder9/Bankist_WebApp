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
    public class AuthController : ControllerBase
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
        public async Task<ActionResult<MyAuthInfo>> Login([FromBody] AuthLoginVM request, [FromQuery] string token)
        {
            try
            {
                var authInfo = await _authService.Login(request, token);
                return Ok(authInfo);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
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
        [HttpPost("2fKey")]
        public async Task<ActionResult<NoResponse>> Unlocked([FromBody] Auth2FRequest request)
        {
            if (!_authService.GetAuthInfo().isLogin)
            {
                throw new Exception("User is not logged in");
            }
            var token = _authService.GetAuthInfo().autentificationToken;
            if (token == null)
                return BadRequest("Authentication token is null.");

            if (request.key == token.TwoFKey)
            {
                token.Is2FAUnlocked = true;
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                return BadRequest("not valid key");
            }
            return Ok(new NoResponse());
        }
    }
}
