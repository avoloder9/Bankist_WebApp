using API.Data;
using API.Data.Models;
using API.Helper;
using API.Helper.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace API.Endpoints.AuthEndpoints.Logout
{
    public class AuthLogoutEndpoint:MyBaseEndpoint<NoRequest,NoResponse>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;
        public AuthLogoutEndpoint(ApplicationDbContext dbContext, MyAuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }
        [HttpPost]
        public override async Task<NoResponse> Procces([FromBody]NoRequest request, CancellationToken cancellationToken)
        {
           AutentificationToken? autentificationToken=_authService.GetAuthInfo().autentificationToken;
            if(autentificationToken== null)  
            return new NoResponse();

            _dbContext.Remove(autentificationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);
            return new NoResponse();
        }
    }
}
