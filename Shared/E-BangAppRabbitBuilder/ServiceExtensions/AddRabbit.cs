using E_BangAppRabbitBuilder.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace E_BangAppRabbitBuilder.ServiceExtensions
{
    public static class AddRabbit
    {
        public static void AddRabbitService(this IServiceCollection services)
        {
            services.AddScoped<IRabbitRepository, RabbitRepository>();  
        }
    }
}
