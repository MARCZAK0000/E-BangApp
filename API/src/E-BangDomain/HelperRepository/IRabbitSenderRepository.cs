using E_BangDomain.ModelDtos.MessageSender;

namespace E_BangDomain.HelperRepository
{
    public interface IRabbitSenderRepository
    {
        Task<bool> AddMessageToQueue<T>(RabbitMessageBaseDto<T> parameters, CancellationToken token) where T : class;
    }
}
