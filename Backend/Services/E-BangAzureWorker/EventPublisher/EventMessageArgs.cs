using E_BangAppRabbitSharedClass.AzureRabbitModel;
using E_BangAzureWorker.AzureStrategy;
using RabbitMQ.Client.Events;

namespace E_BangAzureWorker.EventPublisher
{
    public class EventMessageArgs : AsyncEventArgs
    {
        public EventMessageArgs(string accountID, AzureStrategyEnum azureStrategyEnum)
        {
            AccountID = accountID;
            AzureStrategyEnum = azureStrategyEnum;
        }

        public string AccountID { get; set; }
        public AzureStrategyEnum AzureStrategyEnum { get; set; }
    }
}
