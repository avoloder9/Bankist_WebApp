using API.Data;
using API.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace API.Helper.Services
{
    public class MyAuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MyAuthService(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsLogin()
        {

            return GetAuthInfo().isLogin;
        }

        public MyAuthInfo GetAuthInfo()
        {
            string? authToken = _httpContextAccessor.HttpContext!.Request.Headers["my-auth-token"];

            AutentificationToken? _autentificationToken = _dbContext.AutentificationToken
                .Include(x => x.account)
                .SingleOrDefault(x => x.value == authToken);

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
