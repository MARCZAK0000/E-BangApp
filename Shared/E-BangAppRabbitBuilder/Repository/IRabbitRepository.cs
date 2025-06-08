using E_BangAppRabbitBuilder.Options;
using RabbitMQ.Client;

namespace E_BangAppRabbitBuilder.Repository
{
    internal interface IRabbitRepository
    {
        Task<IConnection> CreateConnectionAsync(RabbitOptions options, CancellationToken token);
        Task<IChannel> CreateChannelAsync(IConnection connection, CancellationToken token);
    }
}
