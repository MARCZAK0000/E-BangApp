namespace E_BangAzureWorker.Services
{
    public interface IRabbitMQService
    {
        Task HandleReciverQueueAsync();

        Task HandleSendQueueAsync();

        void HandleDispose();

    }
}
