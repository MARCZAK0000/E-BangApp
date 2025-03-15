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

        public async Task<IConnection> CreateConnectionAsync(CancellationToken token, Action<IRabbitMQSettings>? action = null)
        {
            var rabbitMqSettings = new RabbitMQSettings();
            action?.Invoke(rabbitMqSettings);

            ConnectionFactory connectionFactory = new()
            {
                HostName = rabbitMqSettings.Host,
            };
            return await connectionFactory.CreateConnectionAsync(token);
        }

        public void Dispose(params IChannel[] channels)
        {
            channels.ToList().ForEach(channel => channel.Dispose());
        }

        public void Dispose(params IConnection[] connecitons)
        {
            connecitons.ToList().ForEach(connection => connection.Dispose());
        }

        public void Dispose(IConnection connection, IChannel channel)
        {
            connection.Dispose();
            channel.Dispose();
        }
    }
}
