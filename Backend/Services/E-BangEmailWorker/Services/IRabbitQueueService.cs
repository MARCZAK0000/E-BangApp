namespace E_BangEmailWorker.Services
{
    public interface IRabbitQueueService
    {
        Task HandleRabbitQueueAsync(CancellationToken token);
    }
}
