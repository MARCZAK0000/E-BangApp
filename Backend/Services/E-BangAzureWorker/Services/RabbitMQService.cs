
using E_BangAzureWorker.EventPublisher;
using E_BangAzureWorker.Repository;
using RabbitMQ.Client;

namespace E_BangAzureWorker.Services
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly IRabbitRepository rabbitRepository;

        private readonly IEventPublisher _eventPublisher;

        private readonly ILogger _logger;
        
        private IConnection? Connection { get; set; }

        private IChannel? SenderChannel { get; set; }

        private IChannel? ReciverChannel { get; set; } 

        public void HandleDispose()
        {
            Connection?.Dispose();
            SenderChannel?.Dispose();
            ReciverChannel?.Dispose();  
        }

        public Task HandleReciverQueueAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task HandleSendQueueAsync(EventMessageArgs args, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
