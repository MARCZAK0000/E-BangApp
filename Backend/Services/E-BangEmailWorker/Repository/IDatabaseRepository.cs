using E_BangAppRabbitSharedClass.BuildersDto.RabbitMessageChilds;
using E_BangAppRabbitSharedClass.RabbitModel;

namespace E_BangEmailWorker.Repository
{
    public interface IDatabaseRepository
    {
        Task SaveEmailInfo(EmailBody emailServiceRabbitMessageModel, bool isSend, CancellationToken token);

    }
}
