using E_BangAppRabbitSharedClass.RabbitModel;

namespace E_BangNotificationService.Repository
{
    public interface IPostgresDbRepostiory
    {
        Task<bool> SaveNotificationAsync(NotificationRabbitMessageModel notification, CancellationToken cancellationToken);
    }
}
