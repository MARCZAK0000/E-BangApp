using Microsoft.EntityFrameworkCore;

namespace NotificationEntities
{
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected NotificationDbContext()
        {
        }

        public DbSet<Notifcation> Notifcations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Notifcation>()
                .HasKey(pr=>pr.NotificationId);

            modelBuilder.Entity<Notifcation>()
                .HasIndex(pr => pr.SenderId);

            modelBuilder.Entity<Notifcation>()
               .HasIndex(pr => pr.ReciverId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
