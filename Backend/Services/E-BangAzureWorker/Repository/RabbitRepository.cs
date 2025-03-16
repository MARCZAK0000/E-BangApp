using E_BangAzureWorker.Model;
using RabbitMQ.Client;

namespace E_BangAzureWorker.Repository
{
    public class RabbitRepository : IRabbitRepository
    {
        public async Task<IChannel> CreateChannelAsync(IConnection connection, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(connection);
            return await connection.CreateChannelAsync(null, token);
        }

        public async Task<IConnection> CreateConnectionAsync(CancellationToken token, Action<RabbitMQSettings>? action = null)
        {
            var rabbitMqSettings = new RabbitMQSettings();
            action?.Invoke(rabbitMqSettings);

            ConnectionFactory connectionFactory = new()
            {
                HostName = rabbitMqSettings.Host,
            };
            return await connectionFactory.CreateConnectionAsync(token);
        }

        public void Dispose(IConnection connection, params IChannel[] channels)
        {
            connection.Dispose();
            channels.ToList().ForEach(channel => channel.Dispose());
        }
    }
}
