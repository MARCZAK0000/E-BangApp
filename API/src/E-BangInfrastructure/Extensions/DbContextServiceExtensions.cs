using E_BangDomain.Entities;
using E_BangInfrastructure.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace E_BangInfrastructure.Extensions
{
    public static class DbContextServiceExtensions
    {
        public static void AddProjectDbContext(this IServiceCollection services, IConfiguration configuration,bool isDocker)
        {
            services.AddDbContext<ProjectDbContext>(options =>
            {
                options.UseSqlServer(
                    isDocker ?
                configuration.GetConnectionString("DbConnectionString")
                :
                    configuration.GetConnectionString(Environment.GetEnvironmentVariable("PROJECT_DB_CONTEXT")!)
                    );
            });

            services.AddIdentityCore<Account>()
                .AddEntityFrameworkStores<ProjectDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<PendingMigrations>();

            services.AddIdentityApiEndpoints<Account>()
                .AddEntityFrameworkStores<ProjectDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.SignIn.RequireConfirmedAccount = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
            });
        }
    }
}
