using API.Helper.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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

            if (!authService.IsLogin())
            {
                context.Result = new UnauthorizedObjectResult("niste logirani na sistem");
                return;
            }

            MyAuthInfo myAuthInfo = authService.GetAuthInfo();
            await actionLogService.Create(context.HttpContext);
            await next();
        }
    }

}
