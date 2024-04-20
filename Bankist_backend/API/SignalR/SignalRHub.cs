using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    public class SignalRHub:Hub
    {
        public override Task OnConnectedAsync()
        {
            Console.WriteLine(this.Context.ConnectionId);
            return base.OnConnectedAsync();
        }

    }
}
