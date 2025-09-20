using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace BackgrounMessageQueues.QueueComponents.Base
{
    public abstract class QueueHandlerService : IQueueHandlerService
    {
        protected readonly ConcurrentQueue<Func<CancellationToken, Task>> _workitem = new();
        protected readonly SemaphoreSlim _semaphore = new(0);

        public virtual void QueueBackgroundWorkItem(Func<CancellationToken, Task> task)
        {
            ArgumentNullException.ThrowIfNull(task);

            _workitem.Enqueue(task);
            _semaphore.Release();
        }

        public virtual async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            _workitem.TryDequeue(out var result);

            return result!;
        }       
    }
}
