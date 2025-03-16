using RabbitMQ.Client.Events;

namespace E_BangAzureWorker.EventPublisher
{
    public class EventPublisher : IEventPublisher
    {
        public event AsyncEventHandler<EventMessageArgs>? ReceivedMessageAsync;

        public async Task OnRecivedMessage(object? sender, EventMessageArgs args)
        {
            if(ReceivedMessageAsync != null)
            {
                await ReceivedMessageAsync.Invoke(this, args);
            }
        }
    }
}
