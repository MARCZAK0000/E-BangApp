using System.ComponentModel;
using Microsoft.EntityFrameworkCore;

namespace E_BangAzureWorker.Database
{
    public class ServiceDbContext : DbContext
    {
        public ServiceDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Container> Containers { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
