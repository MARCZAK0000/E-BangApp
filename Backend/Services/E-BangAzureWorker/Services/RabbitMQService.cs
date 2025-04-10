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
        private readonly IRabbitRepository _rabbitRepository;

        private readonly IEventPublisher _eventPublisher;

        private readonly ILogger<RabbitMQService> _logger;

        private readonly IAzureStrategy _azureStrategy;

        private readonly IRabbitMQSettings _rabbitMQSettings;

        private readonly IDbStrategy _dbStrategy;

        public RabbitMQService(IRabbitRepository rabbitRepository,
            IEventPublisher eventPublisher,
            ILogger<RabbitMQService> logger,
            IAzureStrategy azureStategy,
            IRabbitMQSettings rabbitMQSettings,
            IDbStrategy dbFactory)
        {
            _rabbitRepository = rabbitRepository;
            _eventPublisher = eventPublisher;
            _logger = logger;
            _azureStrategy = azureStategy;
            _rabbitMQSettings = rabbitMQSettings;
            _dbStrategy = dbFactory;
        }

        private IConnection? Connection { get; set; }

        private IChannel? SenderChannel { get; set; }

        private IChannel? ReciverChannel { get; set; }

        public void HandleDispose()
        {
            _rabbitRepository.Dispose(Connection!, [SenderChannel!, ReciverChannel!]);
        }

        public async Task HandleReciverQueueAsync(CancellationToken cancellationToken)
        {
            if (Connection == null)
            {
                Connection = await _rabbitRepository.CreateConnectionAsync(cancellationToken, pr =>
                {
                    pr.Host = _rabbitMQSettings.Host;
                });
            }
            ReciverChannel = await _rabbitRepository.CreateChannelAsync(Connection, cancellationToken);

            await ReciverChannel.QueueDeclareAsync(queue: _rabbitMQSettings.ReciverQueueName,
                durable: true, exclusive: false, autoDelete: false, arguments: null,
                    noWait: false, cancellationToken);
            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(ReciverChannel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var messageModel = JsonHelper.Deserialize<MessageModel>(message);
                _logger.LogInformation("Message Recived at {DateTime} from AccountID: {Id}", DateTime.Now, messageModel.AccountID);
                var result = await _azureStrategy.AzureBlobStrategy(messageModel, cancellationToken);
                if (result.IsDone)
                {
                    var isDb = await _dbStrategy.StrategyRoundRobin(result.IsDone, result.FileChangesInformations, cancellationToken);
                    if (isDb)
                    {
                        await _eventPublisher.OnRecivedMessage(this, new EventMessageArgs(messageModel.AccountID, messageModel.AzureStrategyEnum));
                    }
                }

                await ReciverChannel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, cancellationToken);
            };

            await ReciverChannel.BasicConsumeAsync("task_queue", autoAck: false, consumer: consumer, cancellationToken);
        }

        public async Task HandleSendQueueAsync(EventMessageArgs args, CancellationToken cancellationToken)
        {
            if (Connection == null)
            {
                Connection = await _rabbitRepository.CreateConnectionAsync(cancellationToken, pr =>
                {
                    pr.Host = _rabbitMQSettings.Host;
                });
            }
            SenderChannel = await _rabbitRepository.CreateChannelAsync(Connection, cancellationToken);
            await SenderChannel.QueueDeclareAsync(queue: "task_queue", durable: true, exclusive: false,
                    autoDelete: false, arguments: null, noWait: false, cancellationToken);
            var message = Notifications.GenerateMessage(args);
            var messageString = JsonHelper.Serialize(message);
            var body = Encoding.UTF8.GetBytes(messageString);
            var properties = new BasicProperties
            {
                Persistent = true
            };

            await SenderChannel.BasicPublishAsync(exchange: string.Empty, routingKey: "task_queue", mandatory: true,
                basicProperties: properties, body: body, cancellationToken);
            _logger.LogInformation("Message send at {DateTime}", DateTime.Now);
        }
    }
}
