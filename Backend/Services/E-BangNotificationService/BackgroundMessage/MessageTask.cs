using App.RabbitBuilder.Options;
using App.RabbitBuilder.Service.Sender;
using App.RabbitSharedClass.UniversalModel;

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

        public Task SendToRabitQueue(RabbitMessageModel parameters, QueueOptions RabbitQueueName, CancellationToken token)
        {
            return _rabbitMQService.AddMessageToQueueAsync(_rabbitOptionsExtended, parameters, RabbitQueueName.Name, token);
        }
    }

    public interface IMessageTask
    {
        Task SendToRabitQueue(RabbitMessageModel parameters, QueueOptions rabbitQueueName, CancellationToken token);
    }
}
