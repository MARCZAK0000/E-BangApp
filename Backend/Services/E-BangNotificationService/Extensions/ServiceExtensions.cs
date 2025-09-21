using BackgroundMessage;
using BackgroundWorker;
using BackgrounMessageQueues;
using BackgrounMessageQueues.QueueComponents;
using Decorator;
using FactoryPattern;
using Microsoft.AspNetCore.SignalR;
using NotificationEntities;
using SignalRHub;
using SignalRTypedHub;
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
            services.AddScoped<EmailQueueHandlerService>();
            services.AddScoped<SMSQueueHandlerService>();
            services.AddScoped<NotificationQueueHandlerService>();

            services.AddScoped<IQueueHandlerService, EmailQueueHandlerService>();
            services.AddScoped<IQueueHandlerService, SMSQueueHandlerService>();
            services.AddScoped<IQueueHandlerService, NotificationQueueHandlerService>();

            /// Register Factory and Strategy Pattern Services
            services.AddSingleton<QueueHandlerFactory>();
            services.AddScoped<IQueueHandlerStrategy, QueueHandlerStrategy>();


            services.AddTransient<IMessageTask, MessageTask>();


            services.AddScoped(sp=>
            {
                var db = sp.GetRequiredService<NotificationDbContext>();
                var queueHandlerStrategy = sp.GetRequiredService<IQueueHandlerStrategy>();
                var rabbitOptionsExtended = sp.GetRequiredService<App.RabbitBuilder.Options.RabbitOptionsExtended>();
                var hubConetxt = sp.GetRequiredService<IHubContext<NotificationHub, INotificationClient>>();
                var messageTask = sp.GetRequiredService<IMessageTask>();

                //loggers 
                ILogger<PushNotification> loggerPush = sp.GetRequiredService<ILogger<PushNotification>>();
                ILogger<SMSNotification> loggerSms = sp.GetRequiredService<ILogger<SMSNotification>>();
                ILogger<EmailNotifications> loggerEmail = sp.GetRequiredService<ILogger<EmailNotifications>>();


                INotificationDecorator pushDecorator = new PushNotification(loggerPush,hubConetxt, db);
                INotificationDecorator smsDecorator = new SMSNotification(loggerSms,pushDecorator);
                INotificationDecorator emailDecorator = new EmailNotifications(loggerEmail,smsDecorator, queueHandlerStrategy, rabbitOptionsExtended, messageTask);
                return emailDecorator;
            });
            return services;
        }
    }
}
