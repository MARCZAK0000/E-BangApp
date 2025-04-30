using E_BangAppRabbitSharedClass.RabbitModel;
using E_BangNotificationService.NotificationEntities;

namespace E_BangNotificationService.Repository
{
    public class PostgresDbRepostiory : IPostgresDbRepostiory
    {
        private readonly NotificationDbContext _notificationDbContext;

        public PostgresDbRepostiory(NotificationDbContext notificationDbContext)
        {
            _notificationDbContext = notificationDbContext;
        }

        public async Task<bool> SaveNotificationAsync(NotificationRabbitMessageModel notification, CancellationToken cancellationToken)
        {
            Notifcation notifcation = new Notifcation()
            {
                ReciverId = notification.ReciverId,
                ReciverName = notification.ReciverName,
                SenderId = notification.SenderId,
                SenderName = notification.SenderName,
                IsReaded = notification.IsReaded,
                Text = notification.Text,
                LastUpdateTime = DateTime.Now,
            };
            await _notificationDbContext.Notifcations.AddAsync(notifcation, cancellationToken);
            await _notificationDbContext.SaveChangesAsync(cancellationToken);    

            return true;    
        }
    }
}
