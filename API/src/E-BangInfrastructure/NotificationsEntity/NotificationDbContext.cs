using E_BangDomain.NotificationEntity;
using Microsoft.EntityFrameworkCore;

namespace E_BangInfrastructure.NotificationsEntity;

public partial class NotificationDbContext : DbContext
{
    public NotificationDbContext()
    {
    }

    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Notifcation> Notifcations { get; set; }

    public virtual DbSet<NotificationSetting> NotificationSettings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=NotificationConnectionString");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notifcation>(entity =>
        {
            entity.HasKey(e => e.NotificationId);

            entity.HasIndex(e => e.ReciverId, "IX_Notifcations_ReciverId");

            entity.HasIndex(e => e.SenderId, "IX_Notifcations_SenderId");
        });

        modelBuilder.Entity<NotificationSetting>(entity =>
        {
            entity.HasKey(e => e.AccountId);

            entity.ToTable("NotificationSettings", "Account");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
