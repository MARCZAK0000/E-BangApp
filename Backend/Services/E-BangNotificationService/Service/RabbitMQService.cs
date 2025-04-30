
using E_BangNotificationService.OptionsPattern;
using E_BangNotificationService.Repository;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using E_BangNotificationService.SignalRHub;
using E_BangNotificationService.SignalRTypedHub;
using E_BangAppRabbitSharedClass.RabbitModel;

namespace E_BangNotificationService.Service
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly ILogger<RabbitMQService> _logger;

        private readonly IRabbitRepository _rabbitRepository;

        private readonly RabbitOptions _rabbitOptions;

        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

        private readonly IPostgresDbRepostiory _postgresDbRepostiory;

        public RabbitMQService(ILogger<RabbitMQService> logger, 
            IRabbitRepository rabbitRepository, 
            RabbitOptions rabbitOptions, 
            IHubContext<NotificationHub, 
                INotificationClient> hubContext, 
            IPostgresDbRepostiory postgresDbRepostiory)
        {
            _logger = logger;
            _rabbitRepository = rabbitRepository;
            _rabbitOptions = rabbitOptions;
            _hubContext = hubContext;
            _postgresDbRepostiory = postgresDbRepostiory;
        }

        public async Task CreateListenerQueueAsync(CancellationToken token)
        {
            IConnection connection = await _rabbitRepository.CreateConnectionAsync(token);
            IChannel channel = await _rabbitRepository.CreateChannelAsync(connection, token);

            await channel.QueueDeclareAsync(queue: _rabbitOptions.QueueName,
                    durable: true, exclusive: false, autoDelete: false, arguments: null,
                        noWait: false, token);
            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var messageModel = JsonSerializer.Deserialize<NotificationRabbitMessageModel>(message);
                _logger.LogInformation("Message Recived at {DateTime} from AccountID: {Id} to AccountID {Id2}", DateTime.Now, messageModel!.SenderId, messageModel!.ReciverId);
                bool IsSaved = await _postgresDbRepostiory.SaveNotificationAsync(messageModel, token);
                if (IsSaved)
                {
                    await _hubContext.Clients.Client(messageModel.ReciverId).RecivedMessage(messageModel);
                    _logger.LogInformation("Notification send at {DateTime} from AccountID: {Id} to AccountId{Id2}", DateTime.Now, messageModel.SenderId, messageModel!.ReciverId);
                }
                else
                {
                    _logger.LogError("Notification not send from AccountID: {Id} to AccountId{Id2}", messageModel.SenderId, messageModel!.ReciverId);
                }
                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, token);
            };

            await channel.BasicConsumeAsync(_rabbitOptions.QueueName, autoAck: false, consumer: consumer, token);
        }
    }
}
