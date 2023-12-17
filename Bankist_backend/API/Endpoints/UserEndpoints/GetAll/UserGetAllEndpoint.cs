using API.Data;
using API.Data.Models;
using API.Helper;
using API.Helper.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints.UserEndpoints.GetAll
{
    [ApiController]
    [Route("[controller]")]
    public class UserGetAllEndpoint : MyBaseEndpoint<UserGetAllRequest, UserGetAllResponse>
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;
        public UserGetAllEndpoint(ApplicationDbContext dbContext, MyAuthService authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }
        [HttpGet]
        public override async Task<UserGetAllResponse> Procces([FromQuery]UserGetAllRequest request, CancellationToken cancellationToken)
        {
 
            var user = await _dbContext.User.OrderByDescending(x => x.id).
                Select(x => new UserGetAllResponseUser()
                {

                    userId = x.id,
                    userName = x.username,
                    firstName = x.firstName,
                    lastName = x.lastName,
                    email = x.email,
                    phone = x.phone,                    
                    birthDate = x.birthDate,
                    registrationDate = x.registrationDate
                })
                .ToListAsync(cancellationToken: cancellationToken);

            return new UserGetAllResponse
            {
                Users = user
            };
        }

    }
}

