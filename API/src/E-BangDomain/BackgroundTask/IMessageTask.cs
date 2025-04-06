using E_BangDomain.ModelDtos.MessageSender;

namespace E_BangDomain.BackgroundTask
{
    public interface IMessageTask
    {
        Task SendToRabbitChannelAsync<T>(RabbitMessageBaseDto<T> parameters, CancellationToken token) where T : class;
    }
}
