using Message;
using Microsoft.AspNetCore.SignalR;
using NotificationEntities;
using SignalRHub;
using SignalRTypedHub;
using System.Text.Json;

namespace Decorator
{
    public class PushNotification : INotificationDecorator
    {
        private readonly ILogger<PushNotification> _logger;

        private readonly IHubContext<NotificationHub,  INotificationClient> _hubContext;

        private readonly NotificationDbContext _notificationDbContext;
        public PushNotification(ILogger<PushNotification> logger, IHubContext<NotificationHub, INotificationClient> hubContext, NotificationDbContext notificationDbContext)
        {
            _logger = logger;
            _hubContext = hubContext;
            _notificationDbContext = notificationDbContext;
        }

        public Task<bool> HandleNotification(RabbitMessageModel parameters, NotificationSettings userNotificationSettings, CancellationToken cancellationToken)
        {
            if (userNotificationSettings.IsPushNotificationEnabled)
            {
                //_hubContext.Clients.User(userNotificationSettings.UserId.ToString())
                //    .ReceiveNotification("You have a new notification!");
            }
            throw new NotImplementedException();
        }
    }
}
