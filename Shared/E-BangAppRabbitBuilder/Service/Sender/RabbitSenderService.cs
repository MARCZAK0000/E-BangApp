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

        /// <summary>
        /// Sends a message to a RabbitMQ queue using simple RabbitOptions configuration.
        /// </summary>
        /// <typeparam name="T">The type of message to send</typeparam>
        /// <param name="rabbitOptions">RabbitMQ connection and queue options</param>
        /// <param name="message">The message object to send</param>
        /// <param name="token">Cancellation token for the operation</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task InitSenderRabbitQueueAsync<T>(RabbitOptions rabbitOptions, T message, CancellationToken token) where T : class
        {
            if (rabbitOptions.SenderQueueName == null)
                throw new ArgumentNullException(nameof(rabbitOptions.SenderQueueName), "SenderQueueName cannot be null");

            await RetryConnection(async () =>
            {
                await InitSenderRabbitQueueCoreAsync(rabbitOptions, rabbitOptions.SenderQueueName, message);
            }, token);
        }

        /// <summary>
        /// Sends a message to a specific RabbitMQ queue using extended RabbitOptions configuration.
        /// </summary>
        /// <typeparam name="T">The type of message to send</typeparam>
        /// <param name="rabbitOptions">Extended RabbitMQ connection options with multiple queues</param>
        /// <param name="message">The message object to send</param>
        /// <param name="rabbitQueueName">The name of the specific queue to send to</param>
        /// <param name="token">Cancellation token for the operation</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task InitSenderRabbitQueueAsync<T>(RabbitOptionsExtended rabbitOptions, T message, string rabbitQueueName, CancellationToken token) where T : class
        {
            QueueOptions? queueOptions = rabbitOptions.SenderQueues?.FirstOrDefault(pr => pr.Name == rabbitQueueName)
                ?? throw new ArgumentNullException($"Queue with name '{rabbitQueueName}' not found in SenderQueues.");

            await RetryConnection(async () =>
            {
                await InitSenderRabbitQueueCoreAsync(rabbitOptions, queueOptions, message);
            }, token);
        }

        private async Task RetryConnection(Func<Task> connection, CancellationToken token, int retries = 5, int timeDelay = 10)
        {
            for (int attempts = 1; attempts <= retries; attempts++)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await connection();
                    _logger.LogInformation("RabbitMQ connected successfully on attempt {Attempt}", attempts);
                    return;
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("RabbitMQ connection cancelled");
                    throw; // Re-throw cancellation
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Attempt {Attempt}: RabbitMQ connection failed - {ErrorMessage}", attempts, ex.Message);

                    if (attempts == retries)
                    {
                        _logger.LogError(ex, "All {MaxAttempts} connection attempts failed", retries);
                        throw;
                    }

                    await Task.Delay(TimeSpan.FromSeconds(timeDelay), token);
                }
            }
        }

        private async Task InitSenderRabbitQueueCoreAsync<T>(RabbitOptionsBase rabbitOptions, QueueOptions queueOptions,
            T message) where T : class
        {
            try
            {
                _logger.LogInformation("Initializing RabbitMQ sender with options: {RabbitOptions}", rabbitOptions.ToString());

                _logger.LogDebug("Creating RabbitMQ connection");
                IConnection connection = await _repository.CreateConnectionAsync(rabbitOptions);
                _logger.LogDebug("RabbitMQ connection created successfully");

                _logger.LogDebug("Creating RabbitMQ channel");
                IChannel channel = await _repository.CreateChannelAsync(connection);
                _logger.LogDebug("RabbitMQ channel created successfully");

                _logger.LogInformation("Declaring queue {QueueName} on host {Host}", queueOptions.QueueName, rabbitOptions.Host);
                await channel.QueueDeclareAsync(
                    queue: queueOptions.QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    noWait: false);

                string rabbitMessage = JsonSerializer.Serialize(message);
                byte[] encodeMessage = Encoding.UTF8.GetBytes(rabbitMessage);

                var properties = new BasicProperties
                {
                    Persistent = true
                };

                await channel.BasicPublishAsync(
                    exchange: string.Empty,
                    routingKey: queueOptions.QueueName, // Use actual queue name instead of hardcoded "task_queue"
                    mandatory: true,
                    basicProperties: properties,
                    body: encodeMessage);

                _logger.LogInformation("Message sent successfully to queue {QueueName}", queueOptions.QueueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send message to RabbitMQ queue {QueueName}: {ErrorMessage}",
                    queueOptions.QueueName, ex.Message);
                throw;
            }
        }
    }
}
