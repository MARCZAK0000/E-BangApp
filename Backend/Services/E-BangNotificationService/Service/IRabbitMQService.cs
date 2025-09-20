namespace Service
{
    public interface IRabbitMQService
    {
        Task<bool> ListenerQueueAsync(CancellationToken token);
    }
}
