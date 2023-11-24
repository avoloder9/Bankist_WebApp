using API.Data;
using API.Helper;
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
        public UserGetAllEndpoint(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]
        public override async Task<UserGetAllResponse> Procces([FromQuery]UserGetAllRequest request, CancellationToken cancellationToken)
        {
            var user = await _dbContext.User.OrderByDescending(x => x.userId).
                Select(x => new UserGetAllResponseUser()
                {

                    userId = x.userId,
                    userName = x.userName,
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

