using E_BangAzureWorker.Model;
using RabbitMQ.Client;

namespace E_BangAzureWorker.Repository
{
    public interface IRabbitRepository
    {
        Task<IConnection> CreateConnectionAsync(CancellationToken token, Action<IRabbitMQSettings>? action = null);
        Task<IChannel> CreateChannelAsync(IConnection connection, CancellationToken token);
        void Dispose(params IChannel[] channels);
        void Dispose(params IConnection[] connections);
        void Dispose(IConnection connection, IChannel channel);
    }
}
