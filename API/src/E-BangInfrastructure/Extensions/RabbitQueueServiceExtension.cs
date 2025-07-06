using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.ServiceExtensions;
using E_BangDomain.BackgroundTask;
using E_BangDomain.IQueueService;
using E_BangInfrastructure.BackgroundTask;
using E_BangInfrastructure.QueueService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangInfrastructure.Extensions
{
    public static class RabbitQueueServiceExtension
    {
        public static void AddRabbitQueueService(this IServiceCollection services, IConfiguration configuration, bool isDocker) 
        {
            services.AddSingleton<IMessageSenderHandlerQueue, MessageSenderHandlerQueue>();
            services.AddTransient<IMessageTask, MessageTask>();
            services.AddRabbitService();

            if (isDocker)
            {
                services.AddOptions<RabbitOptions>()
                    .Configure(cfg =>
                    {
                        cfg.Host = Environment.GetEnvironmentVariable("RABBIT_USERNAME")!;
                        cfg.Port = Convert.ToInt32(Environment.GetEnvironmentVariable("RABBIT_USERNAME")!);
                        cfg.VirtualHost = Environment.GetEnvironmentVariable("RABBIT_USERNAME")!;
                        cfg.UserName = Environment.GetEnvironmentVariable("RABBIT_USERNAME")!;
                        cfg.Password = Environment.GetEnvironmentVariable("RABBIT_USERNAME")!;
                        cfg.ListenerQueueName = string.Empty;
                        cfg.SenderQueueName = string.Empty;
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
        }
    }
}
