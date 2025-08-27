using E_BangAppRabbitBuilder.Configuration;
using E_BangAppRabbitBuilder.Options;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace E_BangAppRabbitBuilder.Repository
{
    public class RabbitRepository : IRabbitRepository
    {
        private readonly ILogger<RabbitRepository> _logger;

        private readonly ConfigurationOptions _configurationOptions;
        public RabbitRepository(ILogger<RabbitRepository> logger, ConfigurationOptions configurationOptions)
        {
            _logger = logger;
            _configurationOptions = configurationOptions;
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

        public async Task<IConnection> CreateConnectionAsync(RabbitOptionsBase options)
        {
            for (int i = 1; i <= _configurationOptions.ConnectionRetryCount; i++)
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
                    _logger.LogInformation("{Date} - RabbitMQ connected on attempt {Attempt}", DateTime.Now, i);
                    return connection;  
                }
                catch (Exception e)
                {
                    _logger.LogWarning("{Date} - Attempt {Attempt}: Connection failed - {ex}", DateTime.Now, i, e.Message);
                    if (i == _configurationOptions.ConnectionRetryCount)
                    {
                        _logger.LogError("All {Max} connection attempts failed.", _configurationOptions.ConnectionRetryCount);
                        throw;
                    }
                    await Task.Delay(TimeSpan.FromSeconds(_configurationOptions.ConnectionRetryDelaySeconds)); 
                }
            }
            throw new Exception("Unable to create RabbitMQ connection.");
        }
    }
}
