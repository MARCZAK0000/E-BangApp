namespace BackgrounMessageQueues
{
    public interface IQueueHandlerService : IDisposable
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> task);
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
