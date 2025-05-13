namespace E_BangAzureWorker.Model
{
    
    public class RabbitMQSettings 
    {
        public string Host { get; set; }
        public string ReciverQueueName { get; set; }
        public string SenderQueueName { get; set; }
    }
}
