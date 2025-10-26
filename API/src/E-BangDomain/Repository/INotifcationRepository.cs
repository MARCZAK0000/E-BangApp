using E_BangDomain.NotificationEntity;

namespace E_BangDomain.Repository
{
    public interface INotifcationRepository
    {
        Task<bool> InsertNotificationSettings(string userId, NotificationSetting notificationSetting, CancellationToken token);
    }
}
