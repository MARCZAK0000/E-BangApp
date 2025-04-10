using E_BangAppRabbitSharedClass.RabbitModel;
using E_BangEmailWorker.EmailMessageBuilderFactory;
using E_BangEmailWorker.Model;
using E_BangEmailWorker.OptionsPattern;
using E_BangEmailWorker.Repository;
using MimeKit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
namespace E_BangEmailWorker.Services
{
    public class RabbitQueueService : IRabbitQueueService
    {
        private readonly IRabbitRepository _rabbitRepository;

        private readonly IEmailRepository _emailRepository;

        private readonly IMessageRepository _messageRepository;

        private readonly IDatabaseRepository _databaseRepository;

        private readonly RabbitOptions _rabbitOptions;

        private readonly IEmailBuilderStrategy _emailBuilderStrategy;

        private readonly ILogger<RabbitQueueService> _logger;
        public RabbitQueueService(IRabbitRepository rabbitRepository,
            IEmailRepository emailRepository,
            IDatabaseRepository databaseRepository,
            RabbitOptions rabbitOptions,
            ILogger<RabbitQueueService> logger,
            IEmailBuilderStrategy emailBuilderStrategy,
            IMessageRepository messageRepository)
        {
            _rabbitRepository = rabbitRepository;
            _emailRepository = emailRepository;
            _databaseRepository = databaseRepository;
            _rabbitOptions = rabbitOptions;
            _logger = logger;
            _emailBuilderStrategy = emailBuilderStrategy;
            _messageRepository = messageRepository;
        }

        public async Task HandleRabbitQueue(CancellationToken token)
        {
            try
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
                    var messageModel = JsonSerializer.Deserialize<EmailServiceRabbitMessageModel>(message);
                    _logger.LogInformation("Message Recived at {DateTime} from AccountID: {Id}", DateTime.Now, messageModel!.AddressTo);
                    string emailRawHTML = _emailBuilderStrategy.EmailBuilderRoundRobin(messageModel.Body);
                    MimeMessage mimeMessage = _messageRepository.BuildMessage(new SendMailDto(messageModel.AddressTo, emailRawHTML, messageModel.Subject), token);
                    bool isSend = await _emailRepository.SendEmailAsync(mimeMessage, token);
                    if (!isSend)
                    {
                        _logger.LogError("There is a problem with email message: {messageModel}", messageModel);
                        throw new Exception("Email Message Problem");
                    }
                    await _databaseRepository.SaveEmailInfo(messageModel, token);
                    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, token);
                };

                await channel.BasicConsumeAsync(_rabbitOptions.QueueName, autoAck: false, consumer: consumer, token);
            }
            catch (Exception ex)
            {
                _logger.LogError("Problem with rabbit handler message: {ex}", ex.Message);
                throw;
            }
            
            
        }
    }
}
