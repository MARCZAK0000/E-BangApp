using E_BangAzureWorker.EventPublisher;

namespace E_BangAzureWorker.Services
{
    public interface IRabbitMQService
    {
        Task HandleReciverQueueAsync(CancellationToken cancellationToken);

        Task HandleSendQueueAsync(EventMessageArgs args, CancellationToken cancellationToken);

    }
}
