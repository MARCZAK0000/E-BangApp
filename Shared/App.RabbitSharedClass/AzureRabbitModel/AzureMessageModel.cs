using App.RabbitSharedClass.Enum;

namespace App.RabbitSharedClass.AzureRabbitModel
{
    public class AzureMessageModel
    {
        public string? AccountID { get; set; }
        public int ContainerID {  get; set; }
        public string? ProductID { get; set; }
        public List<AzureMessageData> Data { get; set; }
        public EAzure AzureStrategyEnum { get; set; }
    }
}
