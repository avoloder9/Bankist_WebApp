using API.Data;
using API.Data.Models;
using API.Endpoints.UserEndpoints.CheckExists;
using API.Endpoints.UserEndpoints.GetAll;
using API.Endpoints.UserEndpoints.Post;
using API.Helper;
using API.Helper.Services;
using API.ViewModels;
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

        [HttpGet("getById")]
        public async Task<ActionResult<UserGetAllVM>> GetUserById(int id)
        {
            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }

            var users = await _dbContext.User.OrderByDescending(x => x.id).Where(x => x.id == id)
                .Select(x => new UserGetAllVMUser
                {
                    userId = x.id,
                    userName = x.username,
                    firstName = x.firstName,
                    lastName = x.lastName,
                    password = x.password,
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

            var userActivity = new UserActivity
            {
                user = user,
                transactionsCount = 0,
                accountStatus = "BRONZE"
            };
            _dbContext.UserActivity.Add(userActivity);

            await _dbContext.SaveChangesAsync();

            return Ok("Congratulations, your account has been successfully created");
        }


        [HttpDelete("close-account")]
        public ActionResult CloseAccount(string username, string bankName, string reason)
        {

            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }

            var user = _dbContext.User.FirstOrDefault(x => x.username == username);

            if (user == null)
            {
                return NotFound("User not found");
            }

            var userCard = _dbContext.BankUserCard.FirstOrDefault(x => x.user.username == username && x.bank.username == bankName);

            if (userCard == null)
            {
                return NotFound("User's card in this bank not found.");
            }

            try
            {
                var deletedUser = new DeletedUser
                {
                    firstName = user.firstName,
                    lastName = user.lastName,
                    email = user.email,
                    phone = user.phone,
                    birthDate = user.birthDate,
                    registrationDate = user.registrationDate,
                    reason = reason,
                    password = user.password,
                    username = user.username,
                    deletionDate = DateTime.Now
                };
                _dbContext.DeletedUser.Add(deletedUser);

                _dbContext.BankUserCard.Remove(userCard);

                _dbContext.SaveChanges();

                return Ok("User's account has been successfully closed in the selected bank.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while closing user's account: {ex.Message}");
            }
        }


        [HttpPut("edit")]
        public async Task<ActionResult> EditUser(UserEditVM user)
        {

            var newUser = await _dbContext.User.FindAsync(user.Id);
            if (newUser == null)
                return NotFound("User not found");
            newUser.firstName = user.firstName;
            newUser.lastName = user.lastName;
            newUser.email = user.email;
            newUser.password = user.password;
            newUser.username = user.userName;
            _dbContext.SaveChanges();
            return Ok();
        }

    }
}
