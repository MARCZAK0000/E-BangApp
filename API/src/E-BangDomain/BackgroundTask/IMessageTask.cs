using App.RabbitSharedClass.UniversalModel;

namespace E_BangDomain.BackgroundTask
{
    public interface IMessageTask
    {
        Task SendToRabbitChannelAsync<T>(RabbitMessageModel<T> parameters, CancellationToken token) where T : class;
    }
}
