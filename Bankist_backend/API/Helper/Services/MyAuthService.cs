using API.Data;
using API.Data.Models;
using API.Endpoints.AuthEndpoints.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json.Serialization;

namespace API.Helper.Services
{
    public class MyAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _cache;
        private readonly MyEmailSenderService _emailSenderService;

        public MyAuthService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor, IMemoryCache cache, MyEmailSenderService emailSenderService)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _cache = cache;
            _emailSenderService = emailSenderService;
        }

        public async Task<MyAuthInfo> Login(AuthLoginVM request)
        {
            var loggedInAccount = await _dbContext.Account.FirstOrDefaultAsync(u => u.username == request.username);

            if (loggedInAccount == null)
            {
                throw new UnauthorizedAccessException("Username not found.");
            }

            if (loggedInAccount.password != request.password)
            {
                throw new UnauthorizedAccessException("Incorrect password.");
            }
            string? twoFKey = null;

            if (loggedInAccount.Is2FActive)
            {
                var user = await _dbContext.User.FirstOrDefaultAsync(u => u.id == loggedInAccount.id);

                if (user != null)
                {
                    twoFKey = TokenGenerator.GenerateRandomKey();
                    _emailSenderService.Send(user.email, "2f", $"Your 2f key is {twoFKey}", false);
                }
            }

            string randomString = TokenGenerator.Generate(10);
            var newToken = new AutentificationToken()
            {
                value = randomString,
                ipAddress = "YourIPAddress",  // Modify this part as needed
                account = loggedInAccount,
                autentificationTimestamp = DateTime.Now,
                TwoFKey = twoFKey,
            };
            _dbContext.Add(newToken);
            await _dbContext.SaveChangesAsync();

            _cache.Set($"ConnectionId_{loggedInAccount.id}", request.SignalRConnectionID);

            return new MyAuthInfo(newToken);
        }

        public bool IsLogin()
        {
            return GetAuthInfo().isLogin;
        }


        public bool isBank()
        {
            return GetAuthInfo().account?.isBank ?? false;
        }
        public bool isUser()
        {
            return GetAuthInfo().account?.isUser ?? false;
        }

        public MyAuthInfo GetAuthInfo()
        {
            string? authToken = _httpContextAccessor.HttpContext!.Request.Headers["Token"];
            Console.WriteLine(authToken);
            AutentificationToken? _autentificationToken = _dbContext.AutentificationToken
                .Include(x => x.account)
                .SingleOrDefault(x => x.value == authToken);
            Console.WriteLine(_autentificationToken);
            return new MyAuthInfo(_autentificationToken);
        }
    }
    public class MyAuthInfo
    {
        public MyAuthInfo(AutentificationToken? autentificationToken)
        {
            this.autentificationToken = autentificationToken;
        }

        [JsonIgnore]
        public Account? account => autentificationToken?.account;
        public AutentificationToken? autentificationToken { get; set; }

        public bool isLogin => account != null;

    }
}
