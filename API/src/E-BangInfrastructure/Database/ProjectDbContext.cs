using E_BangDomain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_BangInfrastructure.Database
{
    public class ProjectDbContext(DbContextOptions options) : IdentityDbContext<Account>(options)
    {
        public DbSet<Account> Account { get; set; }

        public new DbSet<Users> Users { get; set; }

        public DbSet<UserAddress> UserAddresses { get; set; }

        public new DbSet<Roles> Roles { get; set; }

        public DbSet<Actions> Actions { get; set; }

        public DbSet<ActionInRole> ActionInRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Account>(pr =>
            {
                pr.HasIndex(u => u.NormalizedUserName).IsUnique();
                pr.HasIndex(u => u.NormalizedEmail);
                pr.Property(u => u.UserName).HasMaxLength(256);
                pr.Property(u => u.NormalizedUserName).HasMaxLength(256);
                pr.Property(u => u.Email).HasMaxLength(256);
                pr.Property(u => u.NormalizedEmail).HasMaxLength(256);

                pr.ToTable("Account", "Account");
            });

            builder.Entity<Users>(pr =>
            {
                pr.HasKey(pr => pr.UserID);
                pr.HasIndex(pr=>pr.Email);  
                pr.HasIndex(pr=>pr.Surname);

                pr.HasOne(pr => pr.Account)
                    .WithOne(pr => pr.User)
                    .HasForeignKey<Users>(pr => pr.UserID)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);

                pr.HasOne(pr=>pr.Role)
                    .WithMany(pr=>pr.Users)
                    .HasForeignKey(pr=>pr.UserID)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);

                pr.Property(pr=>pr.FirstName).HasMaxLength(50);
                pr.Property(pr=>pr.Surname).HasMaxLength(50);
                pr.Property(pr=>pr.SecondName).HasMaxLength(50);
                pr.Property(pr=>pr.PhoneNumber).HasMaxLength(13);
            });

            builder.Entity<UserAddress>(pr =>
            {
                pr.HasKey(pr => pr.UserID);
                pr.HasIndex(pr => pr.Country);

               
                pr.HasOne(pr => pr.User)
                .WithOne(pr => pr.Address)
                .HasForeignKey<UserAddress>(pr => pr.UserID)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);

                pr.Property(pr => pr.PostalCode).HasMaxLength(7);
                pr.Property(pr => pr.Country).HasMaxLength(50);
                pr.Property(pr => pr.City).HasMaxLength(50);
                pr.Property(pr => pr.StreetName).HasMaxLength(50);
                pr.Property(pr => pr.StreetNumber).HasMaxLength(3);
            });

            builder.Entity<ActionInRole>(pr =>
            {
                pr.HasKey(pr=>pr.ActionInRoleID);

                pr.HasOne(pr => pr.Role)
                .WithMany(pr => pr.ActionsInRole)
                .HasForeignKey(pr => pr.RoleID)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Restrict);

                pr.HasOne(pr => pr.Action)
               .WithMany(pr => pr.ActionInRoles)
               .HasForeignKey(pr => pr.ActionID)
               .IsRequired(true)
               .OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<Roles>(pr =>
            {
                pr.HasKey(pr => pr.RoleID);
                pr.HasIndex(pr => pr.RoleName).IsUnique();
                pr.Property(pr => pr.RoleName).HasMaxLength(50);
                pr.Property(pr => pr.RoleDescription).HasMaxLength(150);
            });
            base.OnModelCreating(builder);
        }

    }
}
