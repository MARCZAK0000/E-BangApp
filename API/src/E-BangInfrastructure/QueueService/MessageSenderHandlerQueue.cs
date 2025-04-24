using E_BangDomain.IQueueService;
using System.Collections.Concurrent;

namespace E_BangInfrastructure.QueueService
{
    public class MessageSenderHandlerQueue : IMessageSenderHandlerQueue
    {
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workitem = new();
        private readonly SemaphoreSlim _semaphore = new(0);

        /// <summary>
        /// Remove Message from Queue and Send Task to Background Service
        /// </summary>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>Return taks to Background Service</returns>
        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await _semaphore.WaitAsync(cancellationToken);

            _workitem.TryDequeue(out var result);

            return result!;
        }
        /// <summary>
        /// Add Task To Queue
        /// </summary>
        /// <param name="task">Task to add: <see cref="BackgroundTask.MessageTask"/></param>
        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> task)
        {
            ArgumentNullException.ThrowIfNull(task);

            _workitem.Enqueue(task);
            _semaphore.Release();
        }
    }
}
