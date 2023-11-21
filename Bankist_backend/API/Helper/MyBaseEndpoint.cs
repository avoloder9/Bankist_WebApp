using Microsoft.AspNetCore.Mvc;

namespace API.Helper
{
    public abstract class MyBaseEndpoint<TRequest, TResponse> : ControllerBase
    {
        public abstract Task<TResponse> Procces(TRequest request, CancellationToken cancellationToken);
    }

}
