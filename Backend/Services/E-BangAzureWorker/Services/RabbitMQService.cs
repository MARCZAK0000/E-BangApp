using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.Service.Listener;
using E_BangAppRabbitBuilder.Service.Sender;
using E_BangAppRabbitSharedClass.AzureRabbitModel;
using E_BangAzureWorker.AzureStrategy;
using E_BangAzureWorker.DatabaseFactory;
using E_BangAzureWorker.EventPublisher;
using E_BangAzureWorker.JSON;
using E_BangAzureWorker.Model;
using E_BangAzureWorker.Notification;
using E_BangAzureWorker.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace E_BangAzureWorker.Services
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IEventPublisher _eventPublisher;

        private readonly ILogger<RabbitMQService> _logger;

        private readonly IAzureStrategy _azureStrategy;

        private readonly RabbitOptions _rabbitMQSettings;

        private readonly IDbStrategy _dbStrategy;

        private readonly IRabbitSenderService _rabbitSenderService;

        private readonly IRabbitListenerService _rabbitListenerService;

        public RabbitMQService(IEventPublisher eventPublisher,
            ILogger<RabbitMQService> logger,
            IAzureStrategy azureStategy,
            RabbitOptions rabbitMQSettings,
            IDbStrategy dbFactory,
            IRabbitListenerService rabbitListenerService,
            IRabbitSenderService rabbitSenderService)
        {
            _eventPublisher = eventPublisher;
            _logger = logger;
            _azureStrategy = azureStategy;
            _rabbitMQSettings = rabbitMQSettings;
            _dbStrategy = dbFactory;
            _rabbitListenerService = rabbitListenerService;
            _rabbitSenderService = rabbitSenderService;
        }

        public async Task HandleReciverQueueAsync(CancellationToken cancellationToken)
        {
            await _rabbitListenerService.InitListenerRabbitQueueAsync<AzureMessageModel>(_rabbitMQSettings,
                async (AzureMessageModel messageModel) =>
                {
                    var result = await _azureStrategy.AzureBlobStrategy(messageModel, cancellationToken);
                    if (result.IsDone)
                    {
                        var isDb = await _dbStrategy.StrategyRoundRobin(result.IsDone, result.FileChangesInformations, cancellationToken);
                        if (isDb)
                        {
                            await _eventPublisher.OnRecivedMessage(this, new EventMessageArgs(messageModel.AccountID!, messageModel.AzureStrategyEnum));
                        }
                    }
                }, cancellationToken);
        }

        public async Task HandleSendQueueAsync(EventMessageArgs args, CancellationToken cancellationToken)
        {
            SendModel message = Notifications.GenerateMessage(args);
            await _rabbitSenderService.InitSenderRabbitQueueAsync(_rabbitMQSettings, message, cancellationToken);
        }
    }
}
