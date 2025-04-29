using E_BangNotificationService.SignalRTypedHub;
using Microsoft.AspNetCore.SignalR;

namespace E_BangNotificationService.SignalRHub
{
    public class NotificationHub:Hub<INotificationClient>
    {
    }
}
