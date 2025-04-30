using E_BangNotificationService.OptionsPattern;
using RabbitMQ.Client;

namespace E_BangNotificationService.Repository
{
    public class RabbitRepository : IRabbitRepository
    {
        private readonly RabbitOptions _rabbitOptions;

        public RabbitRepository(RabbitOptions rabbitOptions)
        {
            _rabbitOptions = rabbitOptions;
        }

        public async Task<IChannel> CreateChannelAsync(IConnection connection, CancellationToken token)
            => await connection.CreateChannelAsync(null, token);
        public Task<IConnection> CreateConnectionAsync(CancellationToken token)
        {
            ConnectionFactory connectionFactory = new ConnectionFactory
            {
                HostName = _rabbitOptions.Host
            };
            return connectionFactory.CreateConnectionAsync(token);
        }
    }
}
