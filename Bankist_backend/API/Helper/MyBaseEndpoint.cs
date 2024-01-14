using Microsoft.AspNetCore.Mvc;

public class InvalidToken
{
    public string message { get; set; } 
}

namespace API.Helper
{
    public abstract class MyBaseEndpoint<TRequest, TResponse> : ControllerBase
    {
        public abstract Task<TResponse> Procces(TRequest request, CancellationToken cancellationToken);

        private BadRequestObjectResult BadRequest(string attribute)
        {
            return BadRequest(new
            {
                Message = attribute + "has already been taken"
            });
        }
    }

}
