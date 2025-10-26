using E_BangDomain.NotificationEntity;
using E_BangDomain.Repository;
using E_BangInfrastructure.NotificationsEntity;

namespace E_BangInfrastructure.Repository
{
    public class NotificationRepository : INotifcationRepository
    {
        private readonly NotificationDbContext _notificationDbContext;
        public NotificationRepository(NotificationDbContext notificationDbContext)
        {
            _notificationDbContext = notificationDbContext;
        }
        public async Task<bool> InsertNotificationSettings(string userId, NotificationSetting notificationSetting, CancellationToken token)
        {
            await _notificationDbContext.NotificationSettings.AddAsync(notificationSetting, token);
            return await _notificationDbContext.SaveChangesAsync(token) > 0;
        }
    }
}
