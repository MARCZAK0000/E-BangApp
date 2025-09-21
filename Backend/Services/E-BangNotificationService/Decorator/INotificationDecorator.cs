using NotificationEntities;

namespace Decorator
{
    public interface INotificationDecorator
    {
        Task<bool> HandleNotification<TParameters>(TParameters parameters, 
            NotificationSettings userNotificationSettings, CancellationToken cancellationToken)
            where TParameters : class ,new();
    }
}
