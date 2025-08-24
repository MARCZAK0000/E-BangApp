using Microsoft.EntityFrameworkCore;

namespace E_BangNotificationService.NotificationEntities
{
    public class MigrationHandler
    {
        private readonly NotificationDbContext _notification;

        public MigrationHandler(NotificationDbContext notification)
        {
            _notification = notification;
        }

        public async Task MigrateDb()
        {
            if(await _notification.Database.CanConnectAsync())
            {
                await _notification.Database.MigrateAsync();
            }
        }
    }
}
