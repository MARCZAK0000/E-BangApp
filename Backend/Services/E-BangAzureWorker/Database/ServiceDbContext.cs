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

        public DbSet<BlobContainerRoot> Roots { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
