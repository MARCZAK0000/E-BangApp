using E_BangAppRabbitSharedClass.RabbitModel;

namespace E_BangEmailWorker.Repository
{
    public interface IDatabaseRepository
    {
        Task SaveEmailInfo(EmailServiceRabbitMessageModel emailServiceRabbitMessageModel, CancellationToken token);

    }
}
