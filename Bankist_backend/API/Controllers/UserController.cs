﻿using API.Data;
using API.Data.Models;
using API.Endpoints.UserEndpoints.CheckExists;
using API.Endpoints.UserEndpoints.GetAll;
using API.Endpoints.UserEndpoints.Post;
using API.Helper;
using API.Helper.Auth;
using API.Helper.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [MyAuthorization]
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
        public async Task<ActionResult<UserGetAllByIdVM>> GetUserById(int id, string bankName)
        {
            if (!_authService.IsLogin())
            {
                return Unauthorized();
            }

            var userCardData = await _dbContext.BankUserCard
                .Include(buc => buc.user)
                .Include(buc => buc.card)
                .Include(buc => buc.bank)
                .Where(buc => buc.userId == id)
                .Where(buc => buc.bank.username == bankName)
                .OrderByDescending(buc => buc.id)
                .FirstOrDefaultAsync();

            if (userCardData == null)
            {
                return NotFound();
            }

            var userVM = new UserGetAllByIdVM
            {
                Users = new List<UserGetAllByIdVMUser>
                {
                    new UserGetAllByIdVMUser
                    {
                        userId = userCardData.user.id,
                        userName = userCardData.user.username,
                        firstName = userCardData.user.firstName,
                        lastName = userCardData.user.lastName,
                        email = userCardData.user.email,
                        phone = userCardData.user.phone,
                        password = userCardData.user.password,
                        birthDate = userCardData.user.birthDate,
                        registrationDate = userCardData.user.registrationDate,
                        transactionLimit = userCardData.card.transactionLimit,
                        atmLimit = userCardData.card.atmLimit,
                        negativeLimit = userCardData.card.negativeLimit
                    }

                },
                bankName = userCardData.bank.username

            };

            return Ok(userVM);
        }
        [AllowAnonymous]
        [HttpPost("check")]
        public IActionResult CheckUserExists([FromBody] UserCheckVM request)
        {
            bool userExists = _userService.UserExists(request.userName, request.email);

            return Ok(new { Exists = userExists });
        }
        [AllowAnonymous]
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
        public async Task<ActionResult> EditUser(UserEditVM user, string bankName)
        {

            var newUser = await _dbContext.User.FindAsync(user.Id);
            if (newUser == null)
                return NotFound("User not found");

            var userCard = await _dbContext.BankUserCard.Include(x => x.card).Where(x => x.bank.username == bankName).FirstOrDefaultAsync(buc => buc.userId == user.Id);
            if (userCard != null)
            {
                var card = userCard.card;
                card.transactionLimit = user.transactionLimit;
                card.atmLimit = user.atmLimit;
                card.negativeLimit = user.negativeLimit;
            }
            newUser.firstName = user.firstName;
            newUser.lastName = user.lastName;
            newUser.email = user.email;
            newUser.password = user.password;
            newUser.username = user.userName;
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

    }
}
