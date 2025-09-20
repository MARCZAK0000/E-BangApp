namespace BackgroundMessage
{
    public class MessageTask : IMessageTask
    {
        public Task<bool> SendToRabitQueue<TParameters>(TParameters parameters, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }

    public interface IMessageTask
    {
        Task<bool> SendToRabitQueue<TParameters>(TParameters parameters, CancellationToken token);
    }
}
