using Microsoft.AspNetCore.SignalR;
using SignalRTypedHub;

namespace SignalRHub
{
    public class NotificationHub : Hub<INotificationClient>
    {
    }
}
