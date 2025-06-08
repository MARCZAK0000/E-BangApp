using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.Repository;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace E_BangAppRabbitBuilder.Service.Sender
{
    internal class RabbitSenderService : IRabbitSenderService
    {
        private readonly IRabbitRepository _repository;
        private readonly ILogger<RabbitSenderService> _logger;

        internal RabbitSenderService(IRabbitRepository repository, ILogger<RabbitSenderService> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task InitSenderRabbitQueueAsync<T>(RabbitOptions rabbitOptions, 
            T Message, CancellationToken token) where T : class
        {
            try
            {
                _logger.LogInformation("Sender:Init connection at {date}", DateTime.Now);
                IConnection connection = await _repository.CreateConnectionAsync(rabbitOptions, token);
                _logger.LogInformation("Sender:Created connection at {date}, conn: {conn}", DateTime.Now, connection.ToString());
                _logger.LogInformation("Sender:Init channel at {date}", DateTime.Now);
                IChannel channel = await _repository.CreateChannelAsync(connection, token);
                _logger.LogInformation("Sender:Created channel at {date}, channel: {channel}", DateTime.Now, channel.ToString());
                _logger.LogInformation
                        ("Sender:Init Queue at {Date}, on {host}, queue_name: {name}",
                        DateTime.Now, rabbitOptions.Host, rabbitOptions.QueueName);
                await channel.QueueDeclareAsync(queue: rabbitOptions.QueueName, durable: true, exclusive: false,
                    autoDelete: false, arguments: null, noWait: false, token);
                string message = JsonSerializer.Serialize(Message);
                byte[] encodeMessage = Encoding.UTF8.GetBytes(message);
                var properties = new BasicProperties
                {
                    Persistent = true
                };
                await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "task_queue", mandatory: true,
                    basicProperties: properties, body: encodeMessage, token);
                _logger.LogInformation("Sender:Message send at {DateTime}", DateTime.Now);

            }
            catch (Exception ex)
            {
                _logger.LogError("Sender:Problem with rabbit handler message: {ex}", ex.Message);
                throw;
            }
        }
    }
}
