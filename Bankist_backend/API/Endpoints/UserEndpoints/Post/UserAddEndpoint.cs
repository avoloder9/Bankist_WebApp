using API.Data;
using API.Data.Models;
using API.Helper;
using Azure.Messaging;
using Microsoft.AspNetCore.Hosting.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace API.Endpoints.UserEndpoints.Post
{
    [ApiController]
    [Route("[controller]")]
    public class UserAddEndpoint : MyBaseEndpoint<UserAddRequest, ActionResult<User>>
    {
        private readonly ApplicationDbContext _dbContext;
        public UserAddEndpoint(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpPost]
        public override async Task<ActionResult<User>> Procces([FromBody] UserAddRequest request, CancellationToken cancellationToken)
        {
            if (_dbContext.User.Any(x => x.username == request.userName))
            {
                return BadRequest("Username has already been taken");
            }

            if (_dbContext.User.Any(x => x.email == request.email))
            {
                return BadRequest("Email has already been taken");
            }

            var pw = checkPasswordStrength(request.password);
            if (!string.IsNullOrEmpty(pw))
            {
                return BadRequest(new { Message = pw.ToString() });
            }

            var phoneNumber = CheckPhoneNumber(request.phone);
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                return BadRequest(new { Message = phoneNumber });
            }
            var user = new User
            {
                firstName = request.firstName,
                lastName = request.lastName,
                email = request.email,
                username = request.userName,
                password = request.password,
                phone = request.phone,
                birthDate = request.birthDate,
                registrationDate = DateTime.Now
            };

            _dbContext.User.Add(user);
            await _dbContext.SaveChangesAsync();

            return Ok("Congratulations, your account has been successfully created");
        }

        private string checkPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder();
            if (password.Length < 8)
            {
                sb.Append("Password must have at least 8 characters");
            }

            if (!(Regex.IsMatch(password, "[a-z]") &&
                  Regex.IsMatch(password, "[A-Z]")))
            {
                sb.Append("Password must contain at least one uppercase letter");
            }

            if (!Regex.IsMatch(password, "[0-9]"))
            {
                sb.Append("Password must contain at least one number");
            }

            if (!(Regex.IsMatch(password, "[<,>,@,!,#,$,%,^,&,*,-,+,/,|,~,=]")))
            {
                sb.Append("Password must contain at least one special character");
            }

            return sb.ToString();
        }
        private string CheckPhoneNumber(string phoneNumber)
        {
            if (int.TryParse(phoneNumber, out _))
            {
                if (!(phoneNumber.Length == 9 || phoneNumber.Length == 10))
                {
                    return "The phone number must have 9 or 10 digits";
                }
            return string.Empty;
            }
            else
            {
                return "The entered text is not a valid phone number";
            }
        }
    }
}
