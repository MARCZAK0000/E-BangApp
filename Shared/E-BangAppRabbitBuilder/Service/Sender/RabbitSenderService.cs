using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.Repository;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace E_BangAppRabbitBuilder.Service.Sender
{
    public class RabbitSenderService : IRabbitSenderService
    {
        private readonly IRabbitRepository _repository;
        private readonly ILogger<RabbitSenderService> _logger;

        public RabbitSenderService(IRabbitRepository repository, ILogger<RabbitSenderService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task InitSenderRabbitQueueAsync<T>(RabbitOptions rabbitOptions, 
            T message) where T : class
        {
            try
            {
                if (string.IsNullOrEmpty(rabbitOptions.SenderQueueName))
                {
                    ArgumentNullException argumentNullException = new("Sender: Invalid Sender Queue Name");
                    throw argumentNullException;
                }
                _logger.LogInformation("{Date} - Rabbit Options: {rabbitOptions}", DateTime.Now, rabbitOptions.ToString());
                _logger.LogInformation("{Date} - SenderQueue :Init connection", DateTime.Now);
                IConnection connection = await _repository.CreateConnectionAsync(rabbitOptions);
                _logger.LogInformation("{Date} - SenderQueue : Created connection, conn: {conn}", DateTime.Now, connection.ToString());
                _logger.LogInformation("{Date} - SenderQueue : Init channel", DateTime.Now);
                IChannel channel = await _repository.CreateChannelAsync(connection);
                _logger.LogInformation("{Date} - SenderQueue :Created channel, channel: {channel}", DateTime.Now, channel.ToString());
                _logger.LogInformation
                        ("{Date} - SenderQueue : Init Queue, on {host}, queue_name: {name}",
                        DateTime.Now, rabbitOptions.Host, rabbitOptions.SenderQueueName);
                await channel.QueueDeclareAsync(queue: rabbitOptions.SenderQueueName, durable: true, exclusive: false,
                    autoDelete: false, arguments: null, noWait: false);
                string rabbitMessage = JsonSerializer.Serialize(message);
                byte[] encodeMessage = Encoding.UTF8.GetBytes(rabbitMessage);
                var properties = new BasicProperties
                {
                    Persistent = true
                };
                await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "task_queue", mandatory: true,
                    basicProperties: properties, body: encodeMessage);
                _logger.LogInformation("{Date} - SenderQueue : Message send", DateTime.Now);

            }
            catch (Exception ex)
            {
                _logger.LogError("{Date} - SenderQueue : Problem with rabbit handler message: {ex}", DateTime.Now, ex.Message);
                throw;
            }
        }
    }
}
