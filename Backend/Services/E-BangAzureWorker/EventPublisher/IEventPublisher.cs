namespace E_BangAzureWorker.EventPublisher
{
    public interface IEventPublisher
    {
        event EventHandler<EventMessageArgs> MessageReceived;

        void OnRecivedMessage(object? sender, EventMessageArgs args);
    }
}
