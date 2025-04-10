using RabbitMQ.Client;

namespace E_BangEmailWorker.Repository
{
    public class RabbitRepository : IRabbitRepository
    {
       
        public async Task<IChannel> CreateChannelAsync(IConnection connection, CancellationToken token)
        {
            return await connection.CreateChannelAsync(null,token);
        }
        public async Task<IConnection> CreateConnectionAsync(string hostName, CancellationToken token)
        {
            ConnectionFactory factory = new ConnectionFactory()
            {
                HostName = hostName,
            };
            return await factory.CreateConnectionAsync(token);
        }
    }
}
