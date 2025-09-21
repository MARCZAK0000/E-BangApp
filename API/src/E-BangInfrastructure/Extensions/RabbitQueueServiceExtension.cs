using App.RabbitBuilder.Options;
using App.RabbitBuilder.ServiceExtensions;
using E_BangApplication.QueueService;
using E_BangDomain.BackgroundTask;
using E_BangDomain.IQueueService;
using E_BangInfrastructure.BackgroundTask;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace E_BangInfrastructure.Extensions
{
    public static class RabbitQueueServiceExtension
    {
        public static IServiceCollection AddRabbitQueueService(this IServiceCollection services, IConfiguration configuration, bool isDocker)
        {
            services.AddSingleton<IMessageSenderHandlerQueue, MessageSenderHandlerQueue>();
            services.AddTransient<IMessageTask, MessageTask>();
            services.AddRabbitService(cfg =>
            {
                cfg.ServiceRetryCount = 5;
                cfg.ServiceRetryDelaySeconds = 2;
                cfg.ConnectionRetryCount = 3;
                cfg.ConnectionRetryDelaySeconds = 2;
            });

            if (isDocker)
            {
                services.AddOptions<RabbitOptionsExtended>()
                    .Configure(cfg =>
                    {
                        cfg.Host = Environment.GetEnvironmentVariable("RABBIT_HOST")!;
                        cfg.Port = Convert.ToInt32(Environment.GetEnvironmentVariable("RABBIT_PORT")!);
                        cfg.VirtualHost = Environment.GetEnvironmentVariable("RABBIT_VIRTUALHOST")!;
                        cfg.UserName = Environment.GetEnvironmentVariable("RABBIT_USERNAME")!;
                        cfg.Password = Environment.GetEnvironmentVariable("RABBIT_PASSWORD")!;
                        cfg.ListenerQueues = [];
                        cfg.SenderQueues = JsonSerializer.Deserialize<List<QueueOptions>>(Environment.GetEnvironmentVariable("API_LISTENER_QUEUE_OPTIONS")!);
                    })
                    .ValidateDataAnnotations();
            }
            else
            {
                services.AddOptions<RabbitOptionsExtended>()
                    .BindConfiguration("RabbitOptions")
                    .ValidateDataAnnotations();
            }
            services.AddSingleton(pr => pr.GetRequiredService<IOptions<RabbitOptionsExtended>>().Value);
            return services;
        }
    }
}
