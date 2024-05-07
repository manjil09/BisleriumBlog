using Microsoft.AspNetCore.SignalR;

namespace BisleriumBlog.API.SignalRHub
{
    public class NotificationHub: Hub
    {
        
        public override Task OnConnectedAsync()
        {
            
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(string message)
        {
            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveNotification",message);
        }
    }
}
