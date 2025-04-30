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
            await _notification.Database.EnsureCreatedAsync(); 
            if(await _notification.Database.CanConnectAsync())
            {
                var pendingMigrations = await _notification.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    await _notification.Database.MigrateAsync();
                }
            }
        }
    }
}
