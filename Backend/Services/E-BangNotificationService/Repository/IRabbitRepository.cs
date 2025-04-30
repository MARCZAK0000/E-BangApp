using RabbitMQ.Client;

namespace E_BangNotificationService.Repository
{
    public interface IRabbitRepository
    {
        Task<IConnection> CreateConnectionAsync(CancellationToken token);
        Task<IChannel> CreateChannelAsync(IConnection connection, CancellationToken token);
    }
}
