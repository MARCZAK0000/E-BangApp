using E_BangDomain.Entities;
using E_BangInfrastructure.Database;
using E_BangInfrastructure.NotificationsEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_BangInfrastructure.Extensions
{
    public static class DbContextServiceExtensions
    {
        public static IServiceCollection AddProjectDbContext(this IServiceCollection services, IConfiguration configuration, bool isDocker)
        {
            services.AddDbContext<ProjectDbContext>(options =>
            {
                options.UseSqlServer(
                    isDocker ?
                configuration.GetConnectionString
                    (Environment.GetEnvironmentVariable("PROJECT_DB_CONTEXT")!):
                configuration.GetConnectionString("DbConnectionString"));
            });


            services.AddDbContext<NotificationDbContext>(options =>
            {
                string connectionString = isDocker ?
                    Environment.GetEnvironmentVariable("EMAIL_CONNECTION_STRING")! :
                    configuration.GetConnectionString("NotificationConnectionString")!;
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<Seeder>();   

            services.AddIdentityCore<Account>()
                .AddEntityFrameworkStores<ProjectDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<PendingMigrations>();

            services.AddIdentityApiEndpoints<Account>()
                .AddEntityFrameworkStores<ProjectDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
            });


            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisConnectionString");
            });
            return services;
        }
    }
}
