using E_BangDomain.Entities;
using E_BangInfrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_BangInfrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration,bool IsDevelopment)
        {
            services.AddDbContext<ProjectDbContext>(options =>
            {
                options.UseSqlServer(
                    IsDevelopment?
                    configuration.GetConnectionString("DbConnectionString")
                    :
                    configuration.GetConnectionString("production")
                    );
            });

            services.AddIdentityCore<Account>()
                .AddEntityFrameworkStores<ProjectDbContext>()
                .AddDefaultTokenProviders();

            services.AddDataProtection();
            services.AddCustomAuthentication();
            services.AddCustomAuthorization();
            services.AddCorsPolicy(configuration);

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

        public static void AppInfrastructure(this IApplicationBuilder app)
        {
            app.UseEndpoints(pr =>
            {
                pr.MapIdentityApi<Account>();
            });
        }
    }
}
