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

        public async Task<IChannel> CreateChannelAsync(IConnection connection, CancellationToken token)
        {
            try
            {
                return await connection.CreateChannelAsync(null, token);
            }
            catch (Exception e)
            {
                _logger.LogInformation("{Date} - Create Channel: Error - {ex}", DateTime.Now, e.Message);
                throw;
            }
        }

        public async Task<IConnection> CreateConnectionAsync(RabbitOptions options, CancellationToken token)
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
                return await factory.CreateConnectionAsync(token);
            }
            catch (Exception e)
            {
                _logger.LogInformation("{Date} - Create Connection: Error - {ex}", DateTime.Now, e);
                throw;
            }
           
        }
    }
}
