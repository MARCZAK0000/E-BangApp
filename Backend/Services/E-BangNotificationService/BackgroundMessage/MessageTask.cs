using App.RabbitBuilder.Options;
using App.RabbitBuilder.Service.Sender;

namespace BackgroundMessage
{
    public class MessageTask : IMessageTask
    {
        private readonly RabbitOptionsExtended _rabbitOptionsExtended;

        private readonly IRabbitSenderService _rabbitMQService;

        public MessageTask(RabbitOptionsExtended rabbitOptionsExtended, IRabbitSenderService rabbitMQService)
        {
            _rabbitOptionsExtended = rabbitOptionsExtended;
            _rabbitMQService = rabbitMQService;
        }

        public Task SendToRabitQueue<TParameters>(TParameters parameters, QueueOptions RabbitQueueName, CancellationToken token)
            where TParameters : class, new()
        {
            return _rabbitMQService.InitSenderRabbitQueueAsync(_rabbitOptionsExtended, parameters, RabbitQueueName.Name, token);
        }
    }

    public interface IMessageTask
    {
        Task SendToRabitQueue<TParameters>(TParameters parameters, QueueOptions rabbitQueueName, CancellationToken token)
            where TParameters : class, new();
    }
}
