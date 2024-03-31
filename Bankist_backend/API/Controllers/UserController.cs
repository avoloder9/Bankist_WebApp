using API.Data;
using API.Data.Models;
using API.Endpoints.UserEndpoints.CheckExists;
using API.Endpoints.UserEndpoints.GetAll;
using API.Endpoints.UserEndpoints.Post;
using API.Helper;
using API.Helper.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly MyAuthService _authService;
        private readonly IUserService _userService;

        public UserController(MyAuthService authService, ApplicationDbContext dbContext, IUserService userService)
        {
            _dbContext = dbContext;
            _authService = authService;
            _userService = userService;
        }

        [HttpGet("get")]
        public async Task<ActionResult<UserGetAllVM>> GetAllUsers()
        {
            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }

            var users = await _dbContext.User.OrderByDescending(x => x.id)
                .Select(x => new UserGetAllVMUser
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
                .ToListAsync();

            if (users == null || users.Count == 0)
            {
                return NotFound();
            }

            return Ok(new UserGetAllVM { Users = users });
        }

        [HttpPost("check")]
        public IActionResult CheckUserExists([FromBody] UserCheckVM request)
        {
            bool userExists = _userService.UserExists(request.userName, request.email);

            return Ok(new { Exists = userExists });
        }



        [HttpPost("add")]
        public async Task<ActionResult<User>> AddUser([FromBody] UserAddVM request)
        {
            if (_dbContext.User.Any(x => x.username == request.userName))
            {
                return BadRequest("Username has already been taken");
            }

            if (_dbContext.User.Any(x => x.email == request.email))
            {
                return BadRequest("Email has already been taken");
            }

            var pw = ValidationHelper.CheckPasswordStrength(request.password);
            if (!string.IsNullOrEmpty(pw))
            {
                return BadRequest(new { Message = pw });
            }

            var phoneNumber = ValidationHelper.CheckPhoneNumber(request.phone);
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


    }
}
