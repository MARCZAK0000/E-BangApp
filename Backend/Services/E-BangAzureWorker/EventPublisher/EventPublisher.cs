namespace E_BangAzureWorker.EventPublisher
{
    public class EventPublisher : IEventPublisher
    {
        public event EventHandler<EventMessageArgs>? MessageReceived;

        public void OnRecivedMessage(object? sender, EventMessageArgs args)
        {
            MessageReceived?.Invoke(this, args);
        }
    }
}
