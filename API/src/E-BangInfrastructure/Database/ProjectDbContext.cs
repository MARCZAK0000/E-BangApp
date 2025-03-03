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
            builder.Entity<Users>(pr =>
            {
                pr.HasKey(pr => pr.UserID);

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
            });

            builder.Entity<UserAddress>(pr =>
            {
                pr.HasKey(pr => pr.UserID);

                pr.HasOne(pr => pr.User)
                .WithOne(pr => pr.Address)
                .HasForeignKey<UserAddress>(pr => pr.UserID)
                .IsRequired(true)
                .OnDelete(DeleteBehavior.Cascade);
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

            builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins").HasNoKey();
            builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims").HasNoKey();
            builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens").HasNoKey();
            base.OnModelCreating(builder);
        }

    }
}
