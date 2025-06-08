using E_BangEmailWorker.OptionsPattern;
using RabbitMQ.Client;

namespace E_BangEmailWorker.Repository
{
    public class RabbitRepository : IRabbitRepository
    {
        public async Task<IChannel> CreateChannelAsync(IConnection connection, CancellationToken token)
        {
            return await connection.CreateChannelAsync(null,token);
        }
        public async Task<IConnection> CreateConnectionAsync(RabbitOptions rabbit, CancellationToken token)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = rabbit.Host,
                VirtualHost = rabbit.VirtualHost,
                Port = rabbit.Port,
                UserName = rabbit.UserName,
                Password = rabbit.Password,
            };
            return await factory.CreateConnectionAsync(token);
        }
    }
}
