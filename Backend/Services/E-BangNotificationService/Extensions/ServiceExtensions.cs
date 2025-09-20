using BackgroundWorker;
using BackgrounMessageQueues;
using BackgrounMessageQueues.QueueComponents;
using FactoryPattern;
using StrategyPattern;

namespace Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddQueues (this IServiceCollection services)
        {
            /// Register Hosted Services
            services.AddHostedService<MainWorker>();
            services.AddHostedService<EmailWorker>();
            services.AddHostedService<SmsWorker>();


            /// Register Queue Handler Services as Singelton
            /// 
            services.AddSingleton<EmailQueueHandlerService>();
            services.AddSingleton<SMSQueueHandlerService>();
            services.AddSingleton<NotificationQueueHandlerService>();

            services.AddSingleton<IQueueHandlerService, EmailQueueHandlerService>();
            services.AddSingleton<IQueueHandlerService, SMSQueueHandlerService>();
            services.AddSingleton<IQueueHandlerService, NotificationQueueHandlerService>();

            /// Register Factory and Strategy Pattern Services
            services.AddSingleton<QueueHandlerFactory>();
            services.AddSingleton<IQueueHandlerStrategy, QueueHandlerStrategy>();

            return services;
        }
    }
}
