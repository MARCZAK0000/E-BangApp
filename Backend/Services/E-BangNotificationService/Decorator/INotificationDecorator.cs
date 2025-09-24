using Message;
using NotificationEntities;
using System.Text.Json;

namespace Decorator
{
    public interface INotificationDecorator
    {
        Task<bool> HandleNotification(RabbitMessageModel parameters,
            NotificationSettings userNotificationSettings, CancellationToken cancellationToken);
            
    }
}
