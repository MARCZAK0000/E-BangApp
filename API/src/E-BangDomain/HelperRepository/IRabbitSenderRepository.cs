using App.RabbitSharedClass.UniversalModel;

namespace E_BangDomain.HelperRepository
{
    public interface IRabbitSenderRepository
    {
        Task<bool> AddMessageToQueue(RabbitMessageModel parameters, CancellationToken token);
    }
}
