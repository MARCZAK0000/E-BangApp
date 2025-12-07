using Microsoft.EntityFrameworkCore;
using System;

namespace E_BangEmailWorker.Database
{
    public class GeneratorDbContext : DbContext
    {
        public GeneratorDbContext(DbContextOptions<GeneratorDbContext> options) : base(options)
        {
        }

        public GeneratorDbContext()
        {
        }

        // Metoda dla EF Tools (migracji) — tymczasowy connection string
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Tymczasowy connection string dla design-time (migracje)
                optionsBuilder.UseSqlite("Data Source=generator.db");
            }
        }

        public DbSet<AssemblyType> GeneratorAssembly { get; set; }
        public DbSet<AssemblyEntityType> GeneratorEntityType { get; set; }
        public DbSet<AssemblyParametersEntity> GeneratorParametersType { get; set; }
        public DbSet<AssemblyComponentEntity> GeneratorComponentType { get; set; }
        public DbSet<EmailType> GeneratorEmailType { get; set; }
        public DbSet<EmailRender> GeneratorEmailRender { get; set; }
        public DbSet<RenderStrategy> GeneratorRenderStrategy { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AssemblyType>(entity =>
            {
                entity.ToTable("AssemblyTypes");
                entity.HasKey(e => e.AssemblyTypeId);
                entity.HasIndex(e => e.AssemblyName);
                entity.Property(e => e.AssemblyName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.AssemblyPath).IsRequired().HasMaxLength(255);
                entity.Property(e => e.LastUpdateTime).HasDefaultValueSql("datetime('now')");
            });

            modelBuilder.Entity<AssemblyEntityType>(entity =>
            {
                entity.ToTable("AssemblyEntityTypes");
                entity.HasKey(e => e.AssemblyEntityTypeId);
                entity.HasIndex(e => e.AssemblyEntityTypeName);
                entity.Property(e => e.AssemblyEntityTypeName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastUpdateTime).HasDefaultValueSql("datetime('now')");
            });

            modelBuilder.Entity<AssemblyParametersEntity>(entity =>
            {
                entity.ToTable("AssemblyParametersEntities");
                entity.HasKey(e => e.AssemblyParametersEntityId);
                entity.HasIndex(e => e.EntityParametersName);
                entity.Property(e => e.EntityParametersName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.EntityParametersValue).IsRequired().HasMaxLength(500);
                entity.Property(e => e.LastUpdateTime).HasDefaultValueSql("datetime('now')");

                entity.HasOne(e => e.AssemblyEntityType)
                    .WithMany(a => a.AssemblyParameterEntities)
                    .HasForeignKey(d => d.AssemblyEntityTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.AssemblyType)
                    .WithMany(a => a.AssemblyParametersEntities)
                    .HasForeignKey(d => d.AssemblyTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<AssemblyComponentEntity>(entity =>
            {
                entity.ToTable("AssemblyComponentEntities");
                entity.HasKey(e => e.AssemblyComponentEntityId);
                entity.HasIndex(e => e.ComponentName);
                entity.Property(e => e.ComponentName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ComponentValue).IsRequired().HasMaxLength(500);
                entity.Property(e => e.LastUpdateTime).HasDefaultValueSql("datetime('now')");

                entity.HasOne(e => e.AssemblyEntityType)
                    .WithMany(a => a.AssemblyComponentEntities)
                    .HasForeignKey(d => d.AssemblyEntityTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.AssemblyType)
                    .WithMany(a => a.AssemblyComponentEntities)
                    .HasForeignKey(d => d.AssemblyTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<EmailType>(entity =>
            {
                entity.ToTable("EmailTypes");
                entity.HasKey(e => e.EmailTypeId);
                entity.HasIndex(e => e.EmailTypeName);
                entity.Property(e => e.EmailTypeName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastUpdateTime).HasDefaultValueSql("datetime('now')");
            });

            modelBuilder.Entity<RenderStrategy>(entity =>
            {
                entity.ToTable("RenderStrategies");
                entity.HasKey(e => e.RenderStrategyId);
                entity.HasIndex(e => e.RenderStrategyName);
                entity.Property(e => e.RenderStrategyName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastUpdateTime).HasDefaultValueSql("datetime('now')");
            });

            modelBuilder.Entity<EmailRender>(entity =>
            {
                entity.ToTable("EmailRenders");
                entity.HasKey(e => e.EmailRenderId);
                entity.Property(e => e.LastUpdateTime).HasDefaultValueSql("datetime('now')");

                entity.HasOne(e => e.EmailType)
                    .WithMany(a => a.EmailRenders)
                    .HasForeignKey(d => d.EmailTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.RenderStrategy)
                    .WithMany(a => a.EmailRenders)
                    .HasForeignKey(d => d.EmailRenderStrategyId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.HeaderParameters)
                    .WithMany(a => a.HeaderParameters)
                    .HasForeignKey(d => d.HeaderParametersId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.FooterParameters)
                    .WithMany(a => a.FooterParameters)
                    .HasForeignKey(d => d.FooterParametersId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.BodyParameters)
                    .WithMany(a => a.BodyParameters)
                    .HasForeignKey(d => d.BodyParametersId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.HeaderComponenets)
                    .WithMany(a => a.HeaderComponents)
                    .HasForeignKey(d => d.HeaderComponenetsId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.FooterComponenets)
                    .WithMany(a => a.FooterComponents)
                    .HasForeignKey(d => d.FooterComponenetsId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(e => e.BodyComponenets)
                    .WithMany(a => a.BodyComponents)
                    .HasForeignKey(d => d.BodyComponenetsId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
