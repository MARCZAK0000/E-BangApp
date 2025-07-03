using E_BangAppRabbitBuilder.Options;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace E_BangAppRabbitBuilder.Repository
{
    public class RabbitRepository : IRabbitRepository
    {
        private readonly ILogger<RabbitRepository> _logger;

        public RabbitRepository(ILogger<RabbitRepository> logger)
        {
            _logger = logger;
        }

        public async Task<IChannel> CreateChannelAsync(IConnection connection)
        {
            try
            {
                return await connection.CreateChannelAsync();
            }
            catch (Exception e)
            {
                _logger.LogInformation("{Date} - Create Channel: Error - {ex}", DateTime.Now, e.Message);
                throw;
            }
        }

        public async Task<IConnection> CreateConnectionAsync(RabbitOptions options)
        {
            int maxAttempts = 5;
            for (int attempts = 1; attempts <= maxAttempts; attempts++)
            {
                try
                {
                    ConnectionFactory factory = new()
                    {
                        HostName = options.Host,
                        VirtualHost = options.VirtualHost,
                        Port = options.Port,
                        UserName = options.UserName,
                        Password = options.Password,
                        RequestedConnectionTimeout = TimeSpan.FromSeconds(10),
                    };
                    IConnection connection = await factory.CreateConnectionAsync();
                    _logger.LogInformation("RabbitMQ connected on attempt {Attempt}", attempts);
                    return connection;  
                }
                catch (Exception e)
                {
                    _logger.LogWarning("{Date} - Attempt {Attempt}: Connection failed - {ex}", DateTime.Now, attempts, e.Message);
                    if (attempts == maxAttempts)
                    {
                        _logger.LogError("All {Max} connection attempts failed.", maxAttempts);
                        throw;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(2)); 
                }
            }
            throw new Exception("Unable to create RabbitMQ connection.");
        }
    }
}
