using E_BangDomain.BackgroundTask;
using E_BangDomain.Entities;
using E_BangDomain.IQueueService;
using E_BangDomain.Repository;
using E_BangInfrastructure.BackgroundTask;
using E_BangInfrastructure.Database;
using E_BangInfrastructure.QueueService;
using E_BangInfrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_BangInfrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, bool IsDevelopment)
        {
            services.AddDbContext<ProjectDbContext>(options =>
            {
                options.UseSqlServer(
                    IsDevelopment ?
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

            //RabbitMqHandler
            services.AddSingleton<IMessageSenderHandlerQueue, MessageSenderHandlerQueue>();
            services.AddTransient<IMessageTask, MessageTask>();

            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IRabbitSenderRepository , IRabbitSenderRepository>();
            services.AddScoped<IShopRepository, ShopRepostiory>();

        }

    }
}
