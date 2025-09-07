
namespace E_BangAppRabbitSharedClass.AzureRabbitModel
{
    public class AzureMessageModel
    {
        public string? AccountID { get; set; }
        public int ContainerID {  get; set; }
        public string? ProductID { get; set; }
        public List<AzureMessageData> Data { get; set; }
        public AzureStrategyEnum AzureStrategyEnum { get; set; }
    }
}
