using API.Data;
using API.Data.Models;
using API.Helper;
using API.Helper.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Endpoints.AuthEndpoints.Get
{
    [Route("auth")]
    public class AuthGetEndpoint : MyBaseEndpoint<NoRequest, MyAuthInfo>
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;
        public AuthGetEndpoint(ApplicationDbContext dbContext, MyAuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        [HttpPost("get")]
        public override async Task<MyAuthInfo> Procces([FromBody] NoRequest request, CancellationToken cancellationToken)
        {
            AutentificationToken? autentificationToken = _authService.GetAuthInfo().autentificationToken;

            return new MyAuthInfo(autentificationToken);
        }
    }
}
