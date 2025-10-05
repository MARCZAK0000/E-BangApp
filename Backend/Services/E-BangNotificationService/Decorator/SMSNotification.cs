using App.RabbitSharedClass.UniversalModel;
using NotificationEntities;

namespace Decorator
{
    public class SMSNotification : INotificationDecorator
    {
        private readonly INotificationDecorator _next;

        private readonly ILogger<SMSNotification> _logger;

        public SMSNotification(ILogger<SMSNotification> logger, INotificationDecorator next)
        {
            _logger = logger;
            _next = next;
        }
        public async Task<bool> HandleNotification(RabbitMessageModel parameters, NotificationSettings userNotificationSettings, CancellationToken cancellationToken)
        {
            await _next.HandleNotification(parameters, userNotificationSettings, cancellationToken);
            if (userNotificationSettings.IsSmsNotificationEnabled)
            {
                _logger.LogInformation("SMS Notification is enabled. Processing SMS notification.");
            }
            return true;
        }
    }
}
