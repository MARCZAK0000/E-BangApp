using E_BangApplication.QueueService;
using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.ServiceExtensions;
using E_BangDomain.BackgroundTask;
using E_BangDomain.IQueueService;
using E_BangInfrastructure.BackgroundTask;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace E_BangInfrastructure.Extensions
{
    public static class RabbitQueueServiceExtension
    {
        public static IServiceCollection AddRabbitQueueService(this IServiceCollection services, IConfiguration configuration, bool isDocker)
        {
            services.AddSingleton<IMessageSenderHandlerQueue, MessageSenderHandlerQueue>();
            services.AddTransient<IMessageTask, MessageTask>();
            services.AddRabbitService();

            if (isDocker)
            {
                services.AddOptions<RabbitOptions>()
                    .Configure(cfg =>
                    {
                        cfg.Host = Environment.GetEnvironmentVariable("RABBIT_HOST")!;
                        cfg.Port = Convert.ToInt32(Environment.GetEnvironmentVariable("RABBIT_PORT")!);
                        cfg.VirtualHost = Environment.GetEnvironmentVariable("RABBIT_VIRTUALHOST")!;
                        cfg.UserName = Environment.GetEnvironmentVariable("RABBIT_USERNAME")!;
                        cfg.Password = Environment.GetEnvironmentVariable("RABBIT_PASSWORD")!;
                        cfg.ListenerQueueName = //Environment.GetEnvironmentVariable("RABBIT_EMAILQUEUE")!;
                        cfg.SenderQueueName = Environment.GetEnvironmentVariable("RABBIT_EMAILQUEUE")!;
                    })
                    .ValidateDataAnnotations();
            }
            else
            {
                services.AddOptions<RabbitOptions>()
                    .BindConfiguration("Rabbit Options")
                    .ValidateDataAnnotations();
            }
            services.AddSingleton(pr => pr.GetRequiredService<IOptions<RabbitOptions>>().Value);
            return services;
        }
    }
}
