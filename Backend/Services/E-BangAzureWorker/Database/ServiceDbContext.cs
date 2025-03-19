using E_BangAzureWorker.Model;
using Microsoft.EntityFrameworkCore;

namespace E_BangAzureWorker.Database
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<BlobContainer> Containers { get; set; }
        public DbSet<BlobItems> Items { get; set; }
        public DbSet<BlobContainerRoot> ContainerRoot { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlobContainerRoot>(pr =>
            {
                pr.HasKey(pr=>pr.Id);
                pr.HasIndex(pr => pr.RootPath)
                    .IsUnique();
                pr.HasMany(pr => pr.BlobContainers)
                    .WithOne(pr => pr.BlobContainerRoot)
                    .HasForeignKey(pr => pr.BlobRootPathID)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<BlobContainer>(pr =>
            {
                pr.HasKey(x=>x.Id);

                pr.HasIndex(x=>x.Name)
                    .IsUnique();
                pr.HasMany(x=>x.Items)
                    .WithOne(x=>x.Container)
                    .HasForeignKey(x=>x.ContainerID)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<BlobItems>(pr =>
            {
                pr.HasKey(x=>x.BlobItemId);

                pr.HasIndex(x => x.BlobItemId)
                    .IsUnique();
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
