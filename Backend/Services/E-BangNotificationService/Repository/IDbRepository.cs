using E_BangNotificationService.NotificationEntities;
namespace E_BangNotificationService.Repository
{
    public interface IDbRepository
    {
        Task<bool> SaveNotificationAsync(Notifcation notification, CancellationToken cancellationToken);
    }
}
