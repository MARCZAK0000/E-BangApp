using E_BangAzureWorker.AzureFactory;

namespace E_BangAzureWorker.Model
{
    public class MessageModel
    {
        public string AccountID { get; set; }

        public string DataName { get; set; }

        public int ContainerID {  get; set; }
        
        public byte[]? Data { get; set; }

        public AzureStrategyEnum AzureStrategyEnum { get; set; }
    }
}
