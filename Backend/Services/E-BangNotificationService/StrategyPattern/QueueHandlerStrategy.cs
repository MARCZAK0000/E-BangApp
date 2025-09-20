using BackgrounMessageQueues;
using FactoryPattern;

namespace StrategyPattern
{
    /// <summary>
    /// Provides a strategy for handling queues by resolving the appropriate queue handler based on the specified queue
    /// type.
    /// </summary>
    /// <remarks>This class uses a factory to retrieve a dictionary of available queue handlers. The
    /// appropriate handler is resolved based on the provided <see cref="EQueue"/> value. If no handler is found for the
    /// specified queue type, an exception is thrown.</remarks>
    public class QueueHandlerStrategy : IQueueHandlerStrategy
    {
        private readonly QueueHandlerFactory _factory;        
        public QueueHandlerStrategy(QueueHandlerFactory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Retrieves the appropriate queue handler service for the specified queue type.
        /// </summary>
        /// <param name="queueType">The type of the queue for which the handler service is requested.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the  IQueueHandlerService
        /// instance associated with the specified queue type.</returns>
        /// <exception cref="ArgumentException">Thrown if no handler is found for the specified queueType.</exception>
        public Task<IQueueHandlerService> HandleQueueAsync(EQueue queueType)
        {
            var dictionary = _factory.GetQueueHandlers();   
            if (!dictionary.TryGetValue(queueType, out var handler))
            {
                throw new ArgumentException($"No handler found for queue type: {queueType}");
            }
            return Task.FromResult(handler);
        }
    }
}
