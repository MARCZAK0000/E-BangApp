using E_BangAzureWorker.Model;
using RabbitMQ.Client;

namespace E_BangAzureWorker.Repository
{
    public interface IRabbitRepository
    {
        Task<IConnection> CreateConnectionAsync(CancellationToken token, Action<RabbitMQSettings>? action = null);
        Task<IChannel> CreateChannelAsync(IConnection connection, CancellationToken token);
        void Dispose(IConnection connection, params IChannel[] channels);
    }
}
