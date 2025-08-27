namespace E_BangAppRabbitBuilder.Options
{
    public class RabbitOptions : RabbitOptionsBase
    {
        public QueueOptions? ListenerQueueName { get; set; }
        public QueueOptions? SenderQueueName { get; set; }

        public override string ToString()
        {
            return base.ToString() +
                $"{nameof(ListenerQueueName)}: {(string.IsNullOrEmpty(ListenerQueueName?.ToString()) ? "none" : ListenerQueueName.ToString())}, " +
                $"{nameof(SenderQueueName)}: {(string.IsNullOrEmpty(SenderQueueName?.ToString()) ? "none" : SenderQueueName.ToString())}";
        }
    }
}
