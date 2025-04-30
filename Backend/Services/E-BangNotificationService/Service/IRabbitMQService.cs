namespace E_BangNotificationService.Service
{
    public interface IRabbitMQService
    {
        Task CreateListenerQueueAsync(CancellationToken token);
    }
}
