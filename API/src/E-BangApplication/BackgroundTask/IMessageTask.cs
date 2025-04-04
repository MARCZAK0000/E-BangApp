using E_BangDomain.ModelDtos.MessageSender;

namespace E_BangApplication.BackgroundTask
{
    public interface IMessageTask
    {
        Task SendToRabbitChannelAsync<T>(RabbitMessageBaseDto<T> parameters, CancellationToken token) where T : class;
    }
}
