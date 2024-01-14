using API.Data;
using API.Data.Models;
using API.Helper;
using API.Helper.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints.BankEndpoints.GetActiveBanks
{
    [ApiController]
    [Route("[controller]")]
    public class GetActiveBanksEndpoint : MyBaseEndpoint<GetActiveBanksRequest, GetActiveBanksResponse>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetActiveBanksEndpoint(ApplicationDbContext dbContext, MyAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public override async Task<GetActiveBanksResponse> Procces([FromQuery] GetActiveBanksRequest request, CancellationToken cancellationToken)
        {
            if (!_authService.IsLogin())
            {
                Response.StatusCode = 401;
                return new GetActiveBanksResponse();
            }

            string? authToken = _httpContextAccessor.HttpContext!.Request.Headers["Token"];
            var databaseToken = _dbContext.AutentificationToken.Where(token => token.value == authToken).ToList();
            var user = new User();
           if (databaseToken[0] != null)
            {
                user = databaseToken[0].account.User;
                var bankUserCard = _dbContext.BankUserCard.Where(item => item.user == user).ToList();
                var banks = new GetActiveBanksResponse();

                for (int i = 0; i < bankUserCard.Count(); i++)
                {
                    var bank = _dbContext.Bank.FirstOrDefault(b => bankUserCard[i].bankId == b.id);

                    if (bank != null)
                    {
                        var username = bank.username;
                        banks.Banks.Add(new ActiveBank { Name = username });
                    }
                }

                Response.StatusCode = 200;
                return banks;
            }

            Response.StatusCode = 503;
            return new GetActiveBanksResponse();

        }
    }
}
