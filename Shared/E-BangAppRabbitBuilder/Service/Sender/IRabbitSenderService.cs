using E_BangAppRabbitBuilder.Options;

namespace E_BangAppRabbitBuilder.Service.Sender
{

    public interface IRabbitSenderService
    {
        /// <summary>
        /// Send Message to Queue 
        /// <see cref="RabbitOptions"/> Used to send rabbit parameters
        /// <typeparamref name="T"/> Message - Custom Message to send
        /// </summary>
        /// <param name="rabbitOptions">Options</param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task InitSenderRabbitQueueAsync<T>(RabbitOptions rabbitOptions, T message, CancellationToken token) where T : class;
        Task InitSenderRabbitQueueAsync<T>(RabbitOptionsExtended rabbitOptions, T message, string rabbitQueueName,CancellationToken token) where T : class;
    }
}
