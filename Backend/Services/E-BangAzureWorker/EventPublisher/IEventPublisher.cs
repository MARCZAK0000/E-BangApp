using RabbitMQ.Client.Events;

namespace E_BangAzureWorker.EventPublisher
{
    public interface IEventPublisher
    {
        event AsyncEventHandler<EventMessageArgs> ReceivedMessageAsync;

        Task OnRecivedMessage(object? sender, EventMessageArgs args);
    }
}
