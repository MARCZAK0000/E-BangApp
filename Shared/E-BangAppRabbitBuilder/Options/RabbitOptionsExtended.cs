namespace E_BangAppRabbitBuilder.Options
{
    public class RabbitOptionsExtended : RabbitOptionsBase
    {
        List<QueueOptions> ListenerQueues { get; set; }
        List<QueueOptions> SenderQueues { get; set; }

        public override string ToString()
        {
            var listenerQueues = ListenerQueues != null
                ? string.Join(", ", ListenerQueues.Select(q => $"{{Name: {q.Name}, QueueName: {q.QueueName}}}"))
                : "null";
            var senderQueues = SenderQueues != null
                ? string.Join(", ", SenderQueues.Select(q => $"{{Name: {q.Name}, QueueName: {q.QueueName}}}"))
                : "null";

            return $"{base.ToString()}, ListenerQueues: [{listenerQueues}], SenderQueues: [{senderQueues}]";
        }
    }
}
