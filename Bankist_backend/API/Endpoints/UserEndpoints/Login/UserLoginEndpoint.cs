using API.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;

namespace API.Endpoints.UserEndpoints.Login
{
    [ApiController]
    [Route("[controller]")]
    public class UserLoginEndpoint : MyBaseEndpoint<UserLoginRequest, UserLoginResponse>
    {
        private readonly ApplicationDbContext _dbContext;

        public UserLoginEndpoint(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public override async Task<UserLoginResponse> Procces([FromBody] UserLoginRequest request, CancellationToken cancellationToken)
        {
            User? user = await _dbContext.User.FirstOrDefaultAsync(u => u.userName == request.username);

            if (user == null)
            {
                return new UserLoginResponse
                {
                    statusCode = 404,
                    message = "User not found"
                };
            }

            if (user.password != request.password)
            {
                return new UserLoginResponse
                {
                    statusCode = 401,
                    message = "Incorrect password"
                };
            }

            return new UserLoginResponse
            {
                statusCode = 200,
                message = "Successfully logged in!"
            };
        }
    }
}
