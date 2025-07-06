using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.ServiceExtensions;
using E_BangDomain.BackgroundTask;
using E_BangDomain.Entities;
using E_BangDomain.HelperRepository;
using E_BangDomain.IQueueService;
using E_BangDomain.Repository;
using E_BangInfrastructure.BackgroundTask;
using E_BangInfrastructure.Database;
using E_BangInfrastructure.HelperRepository;
using E_BangInfrastructure.QueueService;
using E_BangInfrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace E_BangInfrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            bool IsDocker = Environment.GetEnvironmentVariable("IS_DOCKER") != null 
                && Environment.GetEnvironmentVariable("IS_DOCKER")!.Equals("true", StringComparison.CurrentCulture);

            services.AddDataProtection();
            services.AddCustomAuthentication();
            services.AddCustomAuthorization();
            services.AddCorsPolicy(configuration);

            services.AddProjectDbContext(configuration, IsDocker);
            services.AddRabbitQueueService(configuration, IsDocker);        
            
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IRabbitSenderRepository , IRabbitSenderRepository>();
            services.AddScoped<IShopRepository, ShopRepostiory>();
            services.AddScoped<IProductRepository, ProductRepository>();

            
   
        }

    }
}
