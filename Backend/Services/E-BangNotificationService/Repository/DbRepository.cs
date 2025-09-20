using NotificationEntities;

namespace Repository
{
    public class DbRepository : IDbRepository
    {
        private readonly NotificationDbContext _notificationDbContext;

        public DbRepository(NotificationDbContext notificationDbContext)
        {
            _notificationDbContext = notificationDbContext;
        }

        public async Task<bool> SaveNotificationAsync(Notifcation notifcation, CancellationToken cancellationToken)
        {

            await _notificationDbContext.Notifcations.AddAsync(notifcation, cancellationToken);
            return await _notificationDbContext.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
