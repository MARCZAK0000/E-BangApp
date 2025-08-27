using E_BangAppRabbitBuilder.Options;
using RabbitMQ.Client;

namespace E_BangAppRabbitBuilder.Repository
{
    public interface IRabbitRepository
    {
        Task<IConnection> CreateConnectionAsync(RabbitOptionsBase options);
        Task<IChannel> CreateChannelAsync(IConnection connection);
    }
}
