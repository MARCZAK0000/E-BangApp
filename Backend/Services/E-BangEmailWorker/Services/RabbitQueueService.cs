using E_BangEmailWorker.OptionsPattern;
using E_BangEmailWorker.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;

namespace E_BangEmailWorker.Services
{
    public class RabbitQueueService : IRabbitQueueService
    {
        private readonly IRabbitRepository _rabbitRepository;

        private readonly IEmailRepository _emailRepository;

        private readonly IDatabaseRepository _databaseRepository;

        private readonly RabbitOptions _rabbitOptions;

        private readonly ILogger<RabbitQueueService> _logger;
        public RabbitQueueService(IRabbitRepository rabbitRepository, 
            IEmailRepository emailRepository, 
            IDatabaseRepository databaseRepository,
            RabbitOptions rabbitOptions,
            ILogger<RabbitQueueService> logger)
        {
            _rabbitRepository = rabbitRepository;
            _emailRepository = emailRepository;
            _databaseRepository = databaseRepository;
            _rabbitOptions = rabbitOptions;
            _logger = logger;
        }

        public async Task HandleRabbitQueue(CancellationToken token)
        {
            IConnection connection = await _rabbitRepository.CreateConnectionAsync(_rabbitOptions.Host, token);
            IChannel channel = await _rabbitRepository.CreateChannelAsync(connection, token);

            await channel.QueueDeclareAsync(queue: _rabbitOptions.QueueName,
                durable: true, exclusive: false, autoDelete: false, arguments: null,
                    noWait: false, token);
            AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                //var messageModel = JsonHelper.Deserialize<MessageModel>(message);
                //_logger.LogInformation("Message Recived at {DateTime} from AccountID: {Id}", DateTime.Now, messageModel.AccountID);
                //var result = await _azureStrategy.AzureBlobStrategy(messageModel, token);
                //if (result.IsDone)
                //{
                //    var isDb = await _dbStrategy.StrategyRoundRobin(result.IsDone, result.FileChangesInformations, token);
                //    if (isDb)
                //    {
                //        await _eventPublisher.OnRecivedMessage(this, new EventMessageArgs(messageModel.AccountID, messageModel.AzureStrategyEnum));
                //    }
                //}

                await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, token);
            };

            await channel.BasicConsumeAsync(_rabbitOptions.QueueName, autoAck: false, consumer: consumer, token);
            throw new NotImplementedException();
        }
    }
}
