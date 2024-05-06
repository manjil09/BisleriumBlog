using Microsoft.AspNetCore.SignalR;

namespace BisleriumBlog.API.SignalRHub
{
    public class NotificationHub: Hub<INotificationHub>
    {
        public async Task SendNotification(string message)
        {
            await Clients.Client(Context.ConnectionId).SendNotification(message);
        }
    }
}
