namespace E_BangAppRabbitSharedClass.AzureRabbitModel
{
    public class AzureMessageData
    {
        public string DataName { get; set; }
        public string DataType { get; set; }
        public byte[]? Data { get; set; }
    }
}
