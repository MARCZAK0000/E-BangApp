using BackgrounMessageQueues;
using FactoryPattern;

namespace StrategyPattern
{

    /// <summary>
    /// Defines a strategy for handling different types of queues and returning the appropriate queue handler service.
    /// </summary>
    /// <remarks>Implementations of this interface should provide logic to process the specified queue type
    /// and return an instance of <see cref="IQueueHandlerService"/> that can handle the queue's operations.</remarks>
    public interface IQueueHandlerStrategy
    {
        /// <summary>
        /// Asynchronously retrieves an instance of a queue handler service for the specified queue type.
        /// </summary>
        /// <param name="queueType">The type of the queue to handle. This determines the specific implementation of the queue handler service to
        /// be returned.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an instance of  <see
        /// cref="IQueueHandlerService"/> configured to handle the specified queue type.</returns>
        Task<IQueueHandlerService> HandleQueueAsync (EQueue queueType);
    }
}
