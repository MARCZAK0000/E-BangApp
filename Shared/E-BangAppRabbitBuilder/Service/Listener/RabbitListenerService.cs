using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.Repository;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;

namespace E_BangAppRabbitBuilder.Service.Listener
{
    public class RabbitListenerService : IRabbitListenerService
    {
        private readonly IRabbitRepository _repository;
        private readonly ILogger<RabbitListenerService> _logger;

        public RabbitListenerService(IRabbitRepository repository, ILogger<RabbitListenerService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task InitListenerRabbitQueueAsync<T>(RabbitOptions rabbitOptions, Func<T, Task> MessageHook, CancellationToken token)
            where T : class
        {
            try
            {
                await Task.Delay(10000, token);
                _logger.LogInformation("{Date} - Rabbit Options: {rabbitOptions}", DateTime.Now, rabbitOptions.ToString());
                _logger.LogInformation("{Date} - ListenerQueue : Init connection", DateTime.Now);
                IConnection connection = await _repository.CreateConnectionAsync(rabbitOptions, token);
                _logger.LogInformation("{Date} - ListenerQueue : Created connection, conn: {conn}", DateTime.Now, connection.ToString());
                _logger.LogInformation("{Date} - ListenerQueue : Init channel", DateTime.Now);
                IChannel channel = await _repository.CreateChannelAsync(connection, token);
                _logger.LogInformation("{Date} - ListenerQueue : Created channel, channel: {channel}", DateTime.Now, channel.ToString());
                _logger.LogInformation
                    ("{Date} - ListenerQueue : Init Queue, on {host}, queue_name: {name}",
                    DateTime.Now, rabbitOptions.Host, rabbitOptions.ListenerQueueName);
                await channel.QueueDeclareAsync(queue: rabbitOptions.ListenerQueueName,
                    durable: true, exclusive: false, autoDelete: false, arguments: null,
                        noWait: false, token);
                _logger.LogInformation
                    ("{Date} - ListenerQueue : Created Queue, on {host}, queue_name: {name}",
                    DateTime.Now, rabbitOptions.Host, rabbitOptions.ListenerQueueName);

                AsyncEventingBasicConsumer consumer = new(channel);
                consumer.ReceivedAsync += async (sender, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var messageModel = JsonSerializer.Deserialize<T>(message);
                    ArgumentNullException.ThrowIfNull(messageModel, "Message Null");
                    await MessageHook.Invoke(messageModel!);
                    await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false, token);
                };

                await channel.BasicConsumeAsync(rabbitOptions.ListenerQueueName, autoAck: false, consumer: consumer, token);
            }
            catch (Exception ex)
            {
                _logger.LogError("{Date} - ListenerQueue : Problem with rabbit handler message: {ex}", DateTime.Now, ex.Message);
                throw;
            }
        }
    }
}
