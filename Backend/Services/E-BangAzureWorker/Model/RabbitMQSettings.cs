namespace E_BangAzureWorker.Model
{
    public interface IRabbitMQSettings
    {
        string Host { get; }
        string ReciverQueueName { get; }
        string SenderQueueName { get; }
    }
    public class RabbitMQSettings : IRabbitMQSettings
    {
        public string Host { get; set; }
        public string ReciverQueueName { get; set; }
        public string SenderQueueName { get; set; }
    }
}
