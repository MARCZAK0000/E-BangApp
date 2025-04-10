using RabbitMQ.Client;

namespace E_BangEmailWorker.Repository
{
    public interface IRabbitRepository
    {
        Task <IConnection> CreateConnectionAsync(string hostName, CancellationToken token);
        Task <IChannel> CreateChannelAsync(IConnection connection, CancellationToken token);
    }
}
