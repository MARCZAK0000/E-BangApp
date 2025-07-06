using E_BangDomain.BackgroundTask;
using E_BangDomain.HelperRepository;
using E_BangDomain.IQueueService;
using E_BangDomain.ModelDtos.MessageSender;

namespace E_BangInfrastructure.HelperRepository
{
    public class RabbitSenderRepository : IRabbitSenderRepository
    {
        private readonly IMessageTask _messageTask;
        private readonly IMessageSenderHandlerQueue _queue;

        public RabbitSenderRepository(IMessageTask backgroundTask, IMessageSenderHandlerQueue queue)
        {
            _messageTask = backgroundTask;
            _queue = queue;
        }

        public Task<bool> AddMessageToQueue<T>(RabbitMessageBaseDto<T> parameters, CancellationToken token) where T : class
        {
            _queue.QueueBackgroundWorkItem(async token =>
            {
                await _messageTask.SendToRabbitChannelAsync(parameters, token);
            });
            return Task.FromResult(true);
        }
    }
}
