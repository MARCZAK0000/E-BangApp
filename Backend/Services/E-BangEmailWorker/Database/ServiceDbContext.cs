using Microsoft.EntityFrameworkCore;

namespace E_BangEmailWorker.Database
{
    public class ServiceDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Email> Emails { get; set; }
        public DbSet<EmailSettings> EmailSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Email>(pr =>
            {
                pr.HasKey(x => x.EmailID);
                pr.HasIndex(x => x.EmailAddress);

            });

            modelBuilder.Entity<EmailSettings>(pr =>
            {
                pr.HasKey(pr => pr.Id);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}