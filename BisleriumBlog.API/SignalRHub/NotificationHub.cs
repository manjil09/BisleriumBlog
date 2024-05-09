using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace BisleriumBlog.API.SignalRHub
{
    public class NotificationHub: Hub<INotificationHub>
    {
        private Dictionary<string, List<string>> ConnectedClient = new Dictionary<string, List<string>>();
        public override Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var connectionId = Context.ConnectionId;

            if (!ConnectedClient.ContainsKey(userId))
            {
                ConnectedClient[userId] = new List<string>();
            }

            ConnectedClient[userId].Add(connectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var connectionId = Context.ConnectionId;

            if (ConnectedClient.ContainsKey(userId))
            {
                ConnectedClient[userId].Remove(connectionId);

                if (ConnectedClient[userId].Count == 0)
                {
                    ConnectedClient.Remove(userId);
                }
            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendNotification(string userId, string message)
        {
            if (ConnectedClient.ContainsKey(userId))
            {
                foreach (var connectionId in ConnectedClient[userId])
                {
                    //await Clients.Client(connectionId).SendAsync("ReceiveNotification", message);
                }
            }
        }
    }
}
