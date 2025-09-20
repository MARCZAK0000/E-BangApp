using NotificationEntities;
namespace Repository
{
    public interface IDbRepository
    {
        Task<bool> SaveNotificationAsync(Notifcation notification, CancellationToken cancellationToken);
    }
}
