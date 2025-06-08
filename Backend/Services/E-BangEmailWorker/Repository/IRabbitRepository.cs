using E_BangEmailWorker.OptionsPattern;
using RabbitMQ.Client;

namespace E_BangEmailWorker.Repository
{
    public interface IRabbitRepository
    {
        Task <IConnection> CreateConnectionAsync(RabbitOptions rabbit, CancellationToken token);
        Task <IChannel> CreateChannelAsync(IConnection connection, CancellationToken token);
    }
}
