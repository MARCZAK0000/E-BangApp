using E_BangAppRabbitBuilder.Options;
using RabbitMQ.Client;

namespace E_BangAppRabbitBuilder.Repository
{
    internal class RabbitRepository : IRabbitRepository
    {
        public async Task<IChannel> CreateChannelAsync(IConnection connection, CancellationToken token)
        { 
            return await connection.CreateChannelAsync(null, token);
        }

        public Task<IConnection> CreateConnectionAsync(RabbitOptions options, CancellationToken token)
        {
            ConnectionFactory factory = new()
            {
                HostName = options.Host,
                VirtualHost = options.VirtualHost,
                Port = options.Port,
                UserName = options.UserName,
                Password = options.Password,
            };
            return factory.CreateConnectionAsync(token);
        }
    }
}
