namespace BisleriumBlog.API.SignalRHub
{
    public interface INotificationHub
    {
        Task SendNotification(string message);
    }
}
