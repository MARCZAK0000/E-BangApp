using BackgrounMessageQueues;
using BackgrounMessageQueues.QueueComponents;

namespace FactoryPattern
{
    public class QueueHandlerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public QueueHandlerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Dictionary<EQueue, IQueueHandlerService> GetQueueHandlers()
        {
            return new Dictionary<EQueue, IQueueHandlerService>
            {
                { EQueue.Email,  _serviceProvider.GetRequiredService<EmailQueueHandlerService>()},
                { EQueue.SMS, _serviceProvider.GetRequiredService<SMSQueueHandlerService>() },
                { EQueue.Notification, _serviceProvider.GetRequiredService<NotificationQueueHandlerService>()},
            };
        }
    }
}
