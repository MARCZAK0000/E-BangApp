using E_BangDomain.Entities;
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
        public DbSet<UsersInRole> UsersInRoles { get; set; }
        public DbSet<Shop> Shop { get; set; }
        public DbSet<ShopType> ShopTypes { get; set; }
        public DbSet<ShopBranchesInformations> ShopAddressInformations { get; set; }
        public DbSet<ShopStaff> Staff { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductInformations> ProductInformations { get; set; }
        public DbSet<ProductPrice> ProductPrice { get; set; }
        // public DbSet<ProductHistoryPrice> ProductHistoryPrice { get; set; }

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
                pr.HasIndex(pr => pr.Email);
                pr.HasIndex(pr => pr.Surname);

                pr.HasOne(pr => pr.Account)
                    .WithOne(pr => pr.User)
                    .HasForeignKey<Users>(pr => pr.UserID)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);

                pr.Property(pr => pr.FirstName).HasMaxLength(50);
                pr.Property(pr => pr.Surname).HasMaxLength(50);
                pr.Property(pr => pr.SecondName).HasMaxLength(50);
                pr.Property(pr => pr.PhoneNumber).HasMaxLength(13);
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


            builder.Entity<Roles>(pr =>
            {
                pr.HasKey(pr => pr.RoleID);
                pr.HasIndex(pr => pr.RoleName).IsUnique();
                pr.Property(pr => pr.RoleName).HasMaxLength(50);
                pr.Property(pr => pr.RoleDescription).HasMaxLength(150);
            });

            builder.Entity<UsersInRole>(pr =>
            {
                pr.HasKey(pr => pr.UserInRoleID);
                pr.HasIndex(pr => new { pr.UserID, pr.RoleID }).IsUnique();
                pr.HasOne(pr => pr.Users)
                    .WithMany(pr => pr.UsersInRoles)
                    .HasForeignKey(pr => pr.UserID)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
                pr.HasOne(pr => pr.Roles)
                    .WithMany(pr => pr.UsersInRoles)
                    .HasForeignKey(pr => pr.RoleID)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Shop>(pr =>
            {
                pr.HasIndex(pr => pr.ShopName).IsUnique();
                pr.Property(pr => pr.ShopName).HasMaxLength(100);
                pr.Property(pr => pr.ShopDescription).HasMaxLength(250);

                pr.HasOne(pr => pr.ShopType)
                    .WithMany(pr => pr.Shops)
                    .HasForeignKey(pr => pr.ShopTypeId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ShopType>(pr =>
            {
                pr.HasKey(pr => pr.ShopTypeId);
                pr.HasIndex(pr => pr.ShopTypeName).IsUnique();
                pr.Property(pr => pr.ShopTypeName).HasMaxLength(50);
            });

            builder.Entity<ShopBranchesInformations>(pr =>
            {
                pr.HasKey(pr => pr.ShopBranchId);
                pr.HasOne(pr => pr.Shop)
                    .WithMany(pr => pr.ShopAddressInfromations)
                    .HasForeignKey(pr => pr.ShopID)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Restrict);
                pr.Property(pr => pr.ShopCountry).HasMaxLength(50);
                pr.Property(pr => pr.ShopCity).HasMaxLength(50);
                pr.Property(pr => pr.ShopStreetName).HasMaxLength(50);
                pr.Property(pr => pr.ShopPostalCode).HasMaxLength(7);
            });

            builder.Entity<ShopStaff>(pr =>
            {
                pr.HasKey(pr => pr.ShopStaffId);
                pr.HasOne(pr => pr.Shop)
                    .WithMany(pr => pr.ShopStaff)
                    .HasForeignKey(pr => pr.ShopId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
                pr.HasOne(pr => pr.Users)
                    .WithMany(pr => pr.ShopStaff)
                    .HasForeignKey(pr => pr.AccountId)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<Product>(pr =>
            {
                pr.HasKey(pr => pr.ProductId);
                pr.HasIndex(pr => pr.ProductName).IsUnique();
                pr.Property(pr => pr.ProductName).HasMaxLength(100);
                pr.Property(pr => pr.ProductDescription).HasMaxLength(250);
                pr.HasOne(pr => pr.Shop)
                    .WithMany(pr => pr.Products)
                    .HasForeignKey(pr => pr.ShopID)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            builder.Entity<ProductInformations>(pr =>
            {
                pr.HasKey(pr => pr.ProductID);
                pr.HasOne(pr => pr.Product)
                    .WithMany(pr => pr.ProductInformations)
                    .HasForeignKey(pr => pr.ProductID)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ProductPrice>(pr =>
            {
                pr.HasKey(pr => pr.ProductID);
                pr.HasOne(pr => pr.Product)
                    .WithOne(pr => pr.ProductCountPrice)
                    .HasForeignKey<ProductPrice>(pr => pr.ProductID)
                    .IsRequired(true)
                    .OnDelete(DeleteBehavior.Cascade);

                pr.Property(pr => pr.Price).HasPrecision(2);
            });

            base.OnModelCreating(builder);
        }

    }
}
