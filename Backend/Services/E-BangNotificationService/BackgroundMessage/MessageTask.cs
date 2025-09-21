using App.RabbitBuilder.Options;
using Service;

namespace BackgroundMessage
{
    public class MessageTask : IMessageTask
    {
        private readonly ILogger<MessageTask> _logger;

        private readonly RabbitOptionsExtended _rabbitOptionsExtended;

        private readonly IRabbitMQService _rabbitMQService;

        public MessageTask(ILogger<MessageTask> logger, RabbitOptionsExtended rabbitOptionsExtended, IRabbitMQService rabbitMQService)
        {
            _logger = logger;
            _rabbitOptionsExtended = rabbitOptionsExtended;
            _rabbitMQService = rabbitMQService;
        }

        public async Task<bool> SendToRabitQueue<TParameters>(TParameters parameters, QueueOptions RabbitQueueName, CancellationToken token)
        {
            throw new NotImplementedException();
            //await _rabbitMQService.ListenerQueueAsync(_rabbitOptionsExtended, RabbitQueueName, parameters, token);
        }
    }

    public interface IMessageTask
    {
        Task<bool> SendToRabitQueue<TParameters>(TParameters parameters, QueueOptions rabbitQueueName, CancellationToken token);
    }
}
