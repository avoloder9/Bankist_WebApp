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
using Microsoft.AspNetCore.Http;

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
                Response.StatusCode = 404;
                return new UserLoginResponse
                {
                    message = "User not found"
                };
            }

            if (user.password != request.password)
            {
                Response.StatusCode = 401;
                return new UserLoginResponse
                {
                    message = "Incorrect password"
                };
            }

            Response.StatusCode = 200;
            return new UserLoginResponse
            {
                message = "Successfully logged in!"
            };
        }
    }
}
