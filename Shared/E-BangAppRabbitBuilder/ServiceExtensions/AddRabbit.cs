using E_BangAppRabbitBuilder.Repository;
using E_BangAppRabbitBuilder.Service.Listener;
using E_BangAppRabbitBuilder.Service.Sender;
using Microsoft.Extensions.DependencyInjection;

namespace E_BangAppRabbitBuilder.ServiceExtensions
{
    public static class AddRabbit
    {
        public static void AddRabbitService(this IServiceCollection services)
        {
            services.AddScoped<IRabbitRepository, RabbitRepository>();  
            services.AddScoped<IRabbitListenerService,  RabbitListenerService>();   
            services.AddScoped<IRabbitSenderService, RabbitSenderService>();
        }
    }
}
