using E_BangApplication.BackgroundTask;
using E_BangApplication.Exceptions;
using E_BangDomain.ModelDtos.MessageSender;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;

namespace E_BangInfrastructure.BackgroundTask
{
    public class MessageTask : IMessageTask
    {
        private readonly ILogger<MessageTask> _logger; 
        public MessageTask(ILogger<MessageTask> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Send Message to RabbitMQ
        /// </summary>
        /// <param name="parameters">Message Parameters</param>
        /// <param name="token">Cancelation Token</param>
        /// <returns></returns>
        public async Task SendToRabbitChannelAsync<T>(RabbitMessageBaseDto<T> parameters, CancellationToken token) where T : class
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = "localhost",
                };
                using var connection = await factory.CreateConnectionAsync(token);
                using var channel = await connection.CreateChannelAsync(null, token);

                string queueName = Enum.GetName(parameters.RabbitChannel) ?? string.Empty;
                if (string.IsNullOrWhiteSpace(queueName))
                {
                    throw new InvalidQueueNameException(queueName);
                }
                await channel.QueueDeclareAsync(queue: queueName
                    , durable: true, exclusive: true, autoDelete: true, arguments: null, noWait: false, token);

                var message = parameters.Message.ToString() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(message))
                {
                    throw new EmptyRabbitMessageException(message);
                }
                byte[] messageBody = Encoding.UTF8.GetBytes(message);
                await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, messageBody, token);
            }
            catch (InvalidQueueNameException)
            {
                _logger.LogError("Send To Rabbit Channel: Invalid Queue");
                throw;
            }
            catch (EmptyRabbitMessageException)
            {
                _logger.LogError("Send To Rabbit Channel: Message Is Nul lOr WhiteSpace");
                throw;
            }
            catch(Exception ex)
            {
                _logger.LogError("Send To Rabbit Channel: Something went wrong : {mess}", ex.Message);
            }
            
        }


    }
}
