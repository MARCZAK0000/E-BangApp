using E_BangDomain.Cache;
using E_BangDomain.HelperRepository;
using E_BangDomain.Repository;
using E_BangInfrastructure.Cache;
using E_BangInfrastructure.HelperRepository;
using E_BangInfrastructure.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_BangInfrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            bool IsDocker = Environment.GetEnvironmentVariable("IS_DOCKER") != null
                && Environment.GetEnvironmentVariable("IS_DOCKER")!.Equals("true", StringComparison.CurrentCulture);

            services.AddDataProtection();
            services.AddCustomAuthentication(configuration);
            services.AddCustomAuthorization();
            services.AddCorsPolicy(configuration);

            services.AddProjectDbContext(configuration, IsDocker);
            services.AddRabbitQueueService(configuration, IsDocker);

            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IRabbitSenderRepository, RabbitSenderRepository>();
            services.AddScoped<IShopRepository, ShopRepostiory>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IActionRepository, ActionRepository>();

            return services;
        }
        public static IServiceCollection AddCacheStrategyFactory<TRequest, TResponse>(this IServiceCollection services)
        {
            services.AddScoped<ICacheStrategy<TRequest, TResponse>, AddToCacheStrategy<TRequest, TResponse>>();
            services.AddScoped<ICacheStrategy<TRequest, TResponse>, RemoveFromCacheStrategy<TRequest, TResponse>>();
            services.AddScoped<Func<bool, ICacheStrategy<TRequest, TResponse>>>(sp => key =>
            {
                if (key)
                    return sp.GetRequiredService<AddToCacheStrategy<TRequest, TResponse>>();
                return sp.GetRequiredService<RemoveFromCacheStrategy<TRequest, TResponse>>();
            });
            return services;
        }
    }
}
