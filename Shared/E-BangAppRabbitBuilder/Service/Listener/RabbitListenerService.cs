using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.Repository;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace E_BangAppRabbitBuilder.Service.Listener
{
    internal class RabbitListenerService : IRabbitListenerService
    {
        private readonly IRabbitRepository _repository;
        private readonly ILogger<RabbitListenerService> _logger;

        internal RabbitListenerService(IRabbitRepository repository, ILogger<RabbitListenerService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task InitListenerRabbitQueueAsync<T>(RabbitOptions rabbitOptions, Action MessageHook, CancellationToken token)
            where T : class
        {
            try
            {
                _logger.LogInformation("Listener:Init connection at {date}", DateTime.Now);
                IConnection connection = await _repository.CreateConnectionAsync(rabbitOptions, token);
                _logger.LogInformation("Listener:Created connection at {date}, conn: {conn}", DateTime.Now, connection.ToString());
                _logger.LogInformation("Listener:Init channel at {date}", DateTime.Now);
                IChannel channel = await _repository.CreateChannelAsync(connection, token);
                _logger.LogInformation("Listener:Created channel at {date}, channel: {channel}", DateTime.Now, channel.ToString());
                _logger.LogInformation
                    ("Listener:Init Queue at {Date}, on {host}, queue_name: {name}",
                    DateTime.Now, rabbitOptions.Host, rabbitOptions.QueueName);
                await channel.QueueDeclareAsync(queue: rabbitOptions.QueueName,
                    durable: true, exclusive: false, autoDelete: false, arguments: null,
                        noWait: false, token);
                _logger.LogInformation
                    ("Listener:Created Queue at {Date}, on {host}, queue_name: {name}",
                    DateTime.Now, rabbitOptions.Host, rabbitOptions.QueueName);

                AsyncEventingBasicConsumer consumer = new(channel);
                consumer.ReceivedAsync += async (sender, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var messageModel = JsonSerializer.Deserialize<T>(message);
                    MessageHook.Invoke();
                    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, token);
                };

                await channel.BasicConsumeAsync(rabbitOptions.QueueName, autoAck: false, consumer: consumer, token);
            }
            catch (Exception ex)
            {
                _logger.LogError("Listener:Problem with rabbit handler message: {ex}", ex.Message);
                throw;
            }
        }
    }
}
