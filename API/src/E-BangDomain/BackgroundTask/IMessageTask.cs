using App.RabbitSharedClass.UniversalModel;

namespace E_BangDomain.BackgroundTask
{
    public interface IMessageTask
    {
        Task SendToRabbitChannelAsync(RabbitMessageModel parameters, CancellationToken token);
    }
}
