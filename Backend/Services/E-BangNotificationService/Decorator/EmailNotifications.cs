
using App.RabbitBuilder.Options;
using App.RabbitSharedClass.Email;
using App.RabbitSharedClass.Enum;
using App.RabbitSharedClass.UniversalModel;
using BackgroundMessage;
using BackgrounMessageQueues;
using FactoryPattern;
using NotificationEntities;
using StrategyPattern;

namespace Decorator
{
    public class EmailNotifications : INotificationDecorator
    {
        private readonly ILogger<EmailNotifications> _logger;

        private readonly INotificationDecorator _next;

        private readonly IQueueHandlerStrategy _queueHandlerStrategy;

        private readonly RabbitOptionsExtended _rabbitOptionsExtended;

        private readonly IMessageTask _messageTask;

        public EmailNotifications(ILogger<EmailNotifications> logger, INotificationDecorator next,
            IQueueHandlerStrategy queueHandlerStrategy,
            RabbitOptionsExtended rabbitOptionsExtended,
            IMessageTask messageTask)
        {
            _logger = logger;
            _next = next;
            _queueHandlerStrategy = queueHandlerStrategy;
            _messageTask = messageTask;
            _rabbitOptionsExtended = rabbitOptionsExtended;
        }

        public async Task<bool> HandleNotification(RabbitMessageModel parameters, NotificationSettings notificationSettings, CancellationToken cancellationToken)
        {
            await _next.HandleNotification(parameters, notificationSettings, cancellationToken);
            if (notificationSettings.IsEmailNotificationEnabled)
            {
                _logger.LogInformation("Email Notification is enabled. Processing email notification.");
                _logger.LogInformation("Sending email notification...");
                QueueOptions? queueOptions = _rabbitOptionsExtended.SenderQueues?.
                    FirstOrDefault(q => q.QueueName.Contains(Enum.GetName(ERabbitChannel.EmailChannel)!, StringComparison.CurrentCultureIgnoreCase));
                if (queueOptions == null)
                {
                    _logger.LogError("Email queue configuration not found.");
                    throw new InvalidOperationException("Email queue configuration not found.");
                }
                IQueueHandlerService? queueHandlerTask = await _queueHandlerStrategy.HandleQueueAsync(EQueue.Email);
                if (queueHandlerTask == null)
                {
                    _logger.LogError("Email queue service not found.");
                    throw new InvalidOperationException("Email queue service not found.");
                }
                EmailComponentMessage message = parameters.ToEmail();
                queueHandlerTask.QueueBackgroundWorkItem(async (cancellationToken) =>
                {
                    await _messageTask.SendToRabitQueue(message, queueOptions, cancellationToken);
                });
                _logger.LogInformation("Email notification processed and message enqueued.");
            }
            return true;
        }
    }
}
