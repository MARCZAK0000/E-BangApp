using E_BangAppRabbitBuilder.Configuration;
using E_BangAppRabbitBuilder.Repository;
using E_BangAppRabbitBuilder.Service.Listener;
using E_BangAppRabbitBuilder.Service.Sender;
using Microsoft.Extensions.DependencyInjection;

namespace E_BangAppRabbitBuilder.ServiceExtensions
{
    public static class AddRabbitMqExtensions
    {
        public static void AddRabbitService(this IServiceCollection services, Action<ConfigurationOptions>? configurationOptions = null)
        {
            var options = new ConfigurationOptions();
            configurationOptions?.Invoke(options);

            services.AddSingleton(options);
            services.AddScoped<IRabbitRepository, RabbitRepository>();  
            services.AddScoped<IRabbitListenerService,  RabbitListenerService>();   
            services.AddScoped<IRabbitSenderService, RabbitSenderService>();
        }
    }
}
