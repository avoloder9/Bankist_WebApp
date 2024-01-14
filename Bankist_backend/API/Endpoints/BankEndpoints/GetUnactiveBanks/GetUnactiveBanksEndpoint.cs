using API.Data;
using API.Data.Models;
using API.Helper;
using API.Helper.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints.BankEndpoints.GetUnactiveBanks
{
    [ApiController]
    [Route("[controller]")]
    public class GetUnactiveBanksEndpoint : MyBaseEndpoint<GetUnactiveBanksRequest, GetUnactiveBanksResponse>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetUnactiveBanksEndpoint(ApplicationDbContext dbContext, MyAuthService authService, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _authService = authService;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet]
        public override async Task<GetUnactiveBanksResponse> Procces([FromQuery] GetUnactiveBanksRequest request, CancellationToken cancellationToken)
        {
            if (!_authService.IsLogin())
            {
                Response.StatusCode = 401;
                return new GetUnactiveBanksResponse();
            }

            string? authToken = _httpContextAccessor.HttpContext!.Request.Headers["Token"];
            var databaseToken = _dbContext.AutentificationToken.Where(token => token.value == authToken).ToList();
            var user = new User();
            if (databaseToken[0] != null)
            {
                user = databaseToken[0].account.User;
                var bankUserCard = _dbContext.BankUserCard.Where(item => item.user == user).ToList();
                var banks = _dbContext.Bank.ToList();
                var unactiveBanks = new GetUnactiveBanksResponse();
                
                for(int i = 0; i < banks.Count(); i++)
                {
                    bool contains = false;
                    for(int j = 0; j < bankUserCard.Count(); j++)
                    {
                        if (bankUserCard[j].bankId == banks[i].id)
                        {
                            contains = true;
                            break; 
                        }
                    }
                    if (!contains)
                    {
                        unactiveBanks.Banks.Add(new UnactiveBank() { Name = banks[i].username, NumberOfUsers = banks[i].numberOfUsers });
                    }

                }

                Response.StatusCode = 200;
                return unactiveBanks;
            }

            Response.StatusCode = 503;
            return new GetUnactiveBanksResponse();

        }
    }
}
