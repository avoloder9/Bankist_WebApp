using API.Data;
using API.Data.Models;
using Microsoft.AspNetCore.Http.Extensions;

namespace API.Helper.Services
{
    public class MyActionLogService
    {
        public async Task Create(HttpContext httpContext)
        {
            var authService = httpContext.RequestServices.GetService<MyAuthService>()!;
            var request = httpContext.Request;

            var queryString = request.Query;

            string details = "";
            if (request.HasFormContentType)
            {
                foreach (string key in request.Form.Keys)
                {
                    details += " | " + key + "=" + request.Form[key];
                }
            }

            StreamReader reader = new StreamReader(request.Body);
            string bodyText = await reader.ReadToEndAsync();

            var x = new SystemLogs
            {
                User = authService.GetAuthInfo().account!,
                Timestamp = DateTime.Now,
                QueryPath = request.GetEncodedPathAndQuery(),
                PostData = details + "" + bodyText,
                IpAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString(),
            };

            ApplicationDbContext dbContext = request.HttpContext.RequestServices.GetService<ApplicationDbContext>();

            dbContext.Add(x);
            await dbContext.SaveChangesAsync();
        }
    }
}
