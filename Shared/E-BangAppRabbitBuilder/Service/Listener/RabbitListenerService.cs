using E_BangAppRabbitBuilder.Configuration;
using E_BangAppRabbitBuilder.Exceptions;
using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.Repository;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace E_BangAppRabbitBuilder.Service.Listener
{
    public class RabbitListenerService : IRabbitListenerService
    {
        private readonly IRabbitRepository _repository;
        private readonly ILogger<RabbitListenerService> _logger;
        private readonly ConfigurationOptions _configurationOptions;
        public RabbitListenerService(IRabbitRepository repository, ILogger<RabbitListenerService> logger, ConfigurationOptions configurationOptions)
        {
            _repository = repository;
            _logger = logger;
            _configurationOptions = configurationOptions;
        }

        /// <summary>
        /// Initializes a listener for a RabbitMQ queue and sets up a message processing hook.
        /// </summary>
        /// <remarks>This method establishes a connection to the RabbitMQ server and initializes a
        /// listener for the specified queue. The <paramref name="MessageHook"/> delegate is called asynchronously for
        /// each message received from the queue.</remarks>
        /// <typeparam name="T">The type of the message to be processed by the listener. Must be a reference type.</typeparam>
        /// <param name="rabbitOptions">The configuration options for connecting to RabbitMQ, including the queue name to listen to.</param>
        /// <param name="MessageHook">A delegate that processes messages of type <typeparamref name="T"/>. The delegate is invoked for each
        /// message received.</param>
        /// <param name="token">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="rabbitOptions"/> or its <see cref="RabbitOptions.ListenerQueueName"/> property is
        /// <c>null</c>.</exception>
        public async Task InitListenerRabbitQueueAsync<T>(RabbitOptions rabbitOptions, Func<T, Task> MessageHook, CancellationToken token)
            where T : class
        {
            if(rabbitOptions.ListenerQueueName == null)
            {
                throw new ArgumentNullException(nameof(rabbitOptions.ListenerQueueName), "ListenerQueueName cannot be null.");
            }
            await RetryConnection(async () =>
            {
                await InitListenerRabbitQueueCoreAsync(rabbitOptions, rabbitOptions.ListenerQueueName, MessageHook);
            }, token);
        }
        
        /// <summary>
        /// Initializes a listener for a RabbitMQ queue and sets up a message processing hook.
        /// </summary>
        /// <remarks>This method establishes a connection to the specified RabbitMQ queue and configures
        /// it to invoke the provided <paramref name="MessageHook"/> for each incoming message. The connection attempt
        /// is retried in case of transient failures, respecting the provided <paramref name="token"/> for
        /// cancellation.</remarks>
        /// <typeparam name="T">The type of the message to be processed. Must be a reference type.</typeparam>
        /// <param name="rabbitOptions">The extended RabbitMQ configuration options, including queue definitions.</param>
        /// <param name="rabbitName">The name of the RabbitMQ queue to listen to. Must match a queue defined in <paramref name="rabbitOptions"/>.</param>
        /// <param name="MessageHook">A delegate that processes messages of type <typeparamref name="T"/>. The delegate is invoked for each
        /// message received.</param>
        /// <param name="token">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Thrown if a queue with the specified <paramref name="rabbitName"/> is not found in the <paramref
        /// name="rabbitOptions"/>.</exception>
        public async Task InitListenerRabbitQueueAsync<T>(RabbitOptionsExtended rabbitOptions, string rabbitName, Func<T, Task> MessageHook, CancellationToken token)
            where T : class
        {
            QueueOptions? queueOptions = rabbitOptions.ListenerQueues?.FirstOrDefault(q => q.Name == rabbitName);
            if (queueOptions == null)
            {
                throw new ArgumentException($"Queue with name '{rabbitName}' not found in ListenerQueues.");
            }
            await RetryConnection(async () =>
            {
                await InitListenerRabbitQueueCoreAsync(rabbitOptions, queueOptions, MessageHook);
            }, token);
        }
        /// <summary>
        /// Attempts to establish a connection by executing the specified connection action, retrying on failure.
        /// </summary>
        /// <remarks>This method retries the connection action up to the specified number of attempts,
        /// with a delay between each attempt.  If the connection is successfully established, the method returns
        /// immediately. If all attempts fail, the last exception  encountered is re-thrown.</remarks>
        /// <param name="connectAction">The asynchronous action to execute in order to establish the connection.</param>
        /// <param name="token">A <see cref="CancellationToken"/> used to observe cancellation requests.</param>
        /// <param name="maxAttempts">The maximum number of connection attempts. Defaults to 5.</param>
        /// <param name="delaySeconds">The delay, in seconds, between retry attempts. Defaults to 10.</param>
        /// <returns></returns>
        private async Task RetryConnection(Func<Task> connectAction, CancellationToken token)
        {
            
            for (int i = 1; i <= _configurationOptions.ServiceRetryCount; i++)
            {
                try
                {
                    token.ThrowIfCancellationRequested();
                    await connectAction();
                    _logger.LogInformation("{Date} - RabbitMQ connected on attempt {Attempt}", DateTime.Now, i);
                    return;
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("{Date} - RabbitMQ connection cancelled for queue", DateTime.Now);
                    throw; // Re-throw cancellation
                }
                catch (Exception e)
                {
                    _logger.LogWarning("{Date} - Attempt {Attempt}: Connection failed - {ex}", DateTime.Now, i, e.Message);
                    if (i == _configurationOptions.ServiceRetryCount)
                    {
                        _logger.LogError("All {Max} connection attempts failed.", _configurationOptions.ServiceRetryCount);
                        throw new TooManyRetriesException($"All {_configurationOptions.ServiceRetryCount} connection attempts failed");
                    }
                    await Task.Delay(TimeSpan.FromSeconds(_configurationOptions.ServiceRetryDelaySeconds), token);
                }
            }
        }

        /// <summary>
        /// Initializes and configures a RabbitMQ listener for the specified queue, enabling message consumption.
        /// </summary>
        /// <remarks>This method establishes a connection to RabbitMQ, declares the specified queue, and
        /// starts consuming messages from it. Messages are deserialized into the specified type <typeparamref
        /// name="T"/> and passed to the provided <paramref name="MessageHook"/> for processing. If an error occurs
        /// during message processing, the message is negatively acknowledged and not requeued.</remarks>
        /// <typeparam name="T">The type of the message model expected in the queue. This type must be a reference type.</typeparam>
        /// <param name="rabbitOptions">The RabbitMQ connection options, including host, port, and authentication details.</param>
        /// <param name="queueOptions">The configuration options for the target queue, such as the queue name and other properties.</param>
        /// <param name="MessageHook">A callback function to process messages received from the queue. The function is invoked with the
        /// deserialized message of type <typeparamref name="T"/>.</param>
        /// <returns></returns>
        private async Task InitListenerRabbitQueueCoreAsync<T>(RabbitOptionsBase rabbitOptions, QueueOptions queueOptions, Func<T, Task> MessageHook)
            where T : class
        {
            try
            {
                _logger.LogInformation("{Date} - Rabbit Options: {rabbitOptions}", DateTime.Now, rabbitOptions.ToString());
                _logger.LogInformation("{Date} - ListenerQueue : Init connection", DateTime.Now);
                
                IConnection connection = await _repository.CreateConnectionAsync(rabbitOptions);
                _logger.LogInformation("{Date} - ListenerQueue : Created connection, conn: {conn}", DateTime.Now, connection.ToString());
                
                _logger.LogInformation("{Date} - ListenerQueue : Init channel", DateTime.Now);
                IChannel channel = await _repository.CreateChannelAsync(connection);
                _logger.LogInformation("{Date} - ListenerQueue : Created channel, channel: {channel}", DateTime.Now, channel.ToString());
                
                _logger.LogInformation
                    ("{Date} - ListenerQueue : Init Queue, on {host}, queue_name: {name}",
                    DateTime.Now, rabbitOptions.Host, queueOptions.QueueName);
                    
                await channel.QueueDeclareAsync(queue: queueOptions.QueueName,
                    durable: true, exclusive: false, autoDelete: false, arguments: null,
                        noWait: false);
                        
                _logger.LogInformation
                    ("{Date} - ListenerQueue : Created Queue, on {host}, queue_name: {name}",
                    DateTime.Now, rabbitOptions.Host, queueOptions.QueueName);

                AsyncEventingBasicConsumer consumer = new(channel);
                consumer.ReceivedAsync += async (sender, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        var messageModel = JsonSerializer.Deserialize<T>(message);
                        ArgumentNullException.ThrowIfNull(messageModel, "Message Null");
                        await MessageHook.Invoke(messageModel!);
                        await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                    }
                    catch (Exception messageEx)
                    {
                        _logger.LogError("{Date} - Error processing message: {ex}", DateTime.Now, messageEx.Message);
                        await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
                    }
                };

                await channel.BasicConsumeAsync(queueOptions.QueueName, autoAck: false, consumer: consumer);
                
                _logger.LogInformation("{Date} - ListenerQueue : Successfully started consuming messages from {queueName}", 
                    DateTime.Now, queueOptions.QueueName);
            }
            catch (Exception ex)
            {
                _logger.LogError("{Date} - ListenerQueue : Failed to initialize RabbitMQ listener for queue {queueName}. Error: {ex}. Application will continue without RabbitMQ functionality.", 
                    DateTime.Now, queueOptions?.QueueName ?? "Unknown", ex.Message);
            }
        }
    }
}
