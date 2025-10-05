using App.RabbitSharedClass.UniversalModel;
using NotificationEntities;

namespace Decorator
{
    public interface INotificationDecorator
    {
        Task<bool> HandleNotification(RabbitMessageModel parameters,
            NotificationSettings userNotificationSettings, CancellationToken cancellationToken);

    }
}
