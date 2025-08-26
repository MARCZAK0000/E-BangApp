namespace E_BangAppRabbitBuilder.Options
{
    public class RabbitOptions : RabbitOptionsBase
    {
        public string ListenerQueueName { get; set; }
        public string SenderQueueName { get; set; }

        public override string ToString()
        {
            return base.ToString() +
                $"{nameof(ListenerQueueName)}: {(string.IsNullOrEmpty(ListenerQueueName) ? "none" : ListenerQueueName)}, " +
                $"{nameof(SenderQueueName)}: {(string.IsNullOrEmpty(SenderQueueName) ? "none" : SenderQueueName)}";
        }
    }
}
