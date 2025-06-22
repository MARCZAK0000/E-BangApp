namespace E_BangAppRabbitBuilder.Options
{
    public class RabbitOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string ListenerQueueName { get; set; }
        public string SenderQueueName { get; set; }

        public override string ToString()
        {
            return $"{nameof(Host)}: {Host}, " +
                   $"{nameof(Port)}: {Port}, " +
                   $"{nameof(UserName)}: {UserName}, " +
                   $"{nameof(Password)}: {Password}, " +
                   $"{nameof(VirtualHost)}: {VirtualHost}, " +
                   $"{nameof(ListenerQueueName)}: {(string.IsNullOrEmpty(ListenerQueueName) ? "none" : ListenerQueueName)}, " +
                   $"{nameof(SenderQueueName)}: {(string.IsNullOrEmpty(SenderQueueName) ? "none" : SenderQueueName)}";
        }
    }
}
