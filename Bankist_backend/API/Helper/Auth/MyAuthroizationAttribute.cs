using API.Helper.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace API.Helper.Auth
{
    public class MyAuthorizationAttribute : TypeFilterAttribute
    {
        public MyAuthorizationAttribute() : base(typeof(MyAuthorizationAsyncActionFilter))
        {
        }
    }
    public class MyAuthorizationAsyncActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var authService = context.HttpContext.RequestServices.GetService<MyAuthService>()!;
            var actionLogService = context.HttpContext.RequestServices.GetService<MyActionLogService>()!;

            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (actionDescriptor != null && actionDescriptor.MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any())
            {
                await next();
                return;
            }

            if (!authService.IsLogin())
            {
                context.Result = new UnauthorizedObjectResult("Not logged");
                return;
            }

            MyAuthInfo myAuthInfo = authService.GetAuthInfo();

            if (myAuthInfo.account!.Is2FActive && !myAuthInfo.autentificationToken!.Is2FAUnlocked)
            {
                context.Result = new UnauthorizedObjectResult("You haven't unlocked 2FA");
                return;
            }

            await actionLogService.Create(context.HttpContext);
            await next();
        }
    }

}
