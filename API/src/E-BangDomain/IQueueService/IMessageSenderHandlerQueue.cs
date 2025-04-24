namespace E_BangDomain.IQueueService
{
    public interface IMessageSenderHandlerQueue
    {
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> task);
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
}
