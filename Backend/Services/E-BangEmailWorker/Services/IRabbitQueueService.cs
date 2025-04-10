namespace E_BangEmailWorker.Services
{
    public interface IRabbitQueueService
    {
        Task HandleRabbitQueue(CancellationToken token);
    }
}
