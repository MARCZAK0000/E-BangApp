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

        public void MigrateDb()
        {
            _notification.Database.EnsureCreated(); 
            if(_notification.Database.CanConnect())
            {
                var pendingMigrations = _notification.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                {
                    _notification.Database.Migrate();
                }
            }
        }
    }
}
