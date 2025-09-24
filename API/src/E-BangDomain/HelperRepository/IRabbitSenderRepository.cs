using App.RabbitSharedClass.UniversalModel;

namespace E_BangDomain.HelperRepository
{
    public interface IRabbitSenderRepository
    {
        Task<bool> AddMessageToQueue<T>(RabbitMessageModel<T> parameters, CancellationToken token) where T : class;
    }
}
