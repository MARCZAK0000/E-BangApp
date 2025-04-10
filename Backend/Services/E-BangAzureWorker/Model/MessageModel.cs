using E_BangAzureWorker.AzureStrategy;

namespace E_BangAzureWorker.Model
{
    public class MessageModel
    {
        public string AccountID { get; set; }
        public int ContainerID {  get; set; }
        public string? ProductID { get; set; }
        public List<MessageData> Data { get; set; }
        public AzureStrategyEnum AzureStrategyEnum { get; set; }
    }
}
