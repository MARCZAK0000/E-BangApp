using E_BangAppRabbitBuilder.Options;

namespace E_BangAppRabbitBuilder.Service.Listener
{
    public interface IRabbitListenerService
    {
        /// <summary>
        /// Create Rabbit Listener Queue 
        /// <see cref="RabbitOptions"/> Used to send rabbit parameters
        /// <see cref="Action"/> Message Hook - Custom Logic after getting message
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="rabbitOptions">Custom Logic</param>
        /// <param name="MessageHook"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task InitListenerRabbitQueueAsync<T>(RabbitOptions rabbitOptions, Func<T, Task> MessageHook, CancellationToken token) where T: class;
    }
}
