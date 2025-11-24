using App.RabbitBuilder.Exceptions;
using App.RabbitBuilder.Options;
using App.RabbitBuilder.Service.Listener;
using App.RabbitSharedClass.Notifications;
using App.RabbitSharedClass.UniversalModel;
using AppInfo;
using CustomLogger.Abstraction;
using Decorator;
using Microsoft.EntityFrameworkCore;
using NotificationEntities;
using NotificationExceptions;
using System.Text.Json;

namespace BackgroundWorker
{
    public class MainWorker : BackgroundService
    {
        private readonly IInformations _informations;

        private readonly ICustomLogger<MainWorker> _logger;

        private readonly IRabbitListenerService _rabbitListenerService;

        private readonly RabbitOptionsExtended _rabbitOptionsExtended;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private NotificationDbContext? NotificationDbContext;

        private CancellationToken HandlerStopToken;

        private IServiceScope? _scope;

        private Task? Task;

        private INotificationDecorator? NotificationDecorator;

        public MainWorker(IInformations informations,
            ICustomLogger<MainWorker> logger,
            IRabbitListenerService rabbitListenerService,
            RabbitOptionsExtended rabbitOptionsExtended, IServiceScopeFactory serviceScopeFactory)
        {
            _informations = informations;
            _logger = logger;
            _rabbitListenerService = rabbitListenerService;
            _rabbitOptionsExtended = rabbitOptionsExtended;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _informations.InitTime = DateTime.Now;
            _informations.IsWorking = true;
            _logger.LogInformation("Init Connection");
            return base.StartAsync(cancellationToken);
        }
        public override async Task StopAsync(CancellationToken cancellationToken)
        {

            if (Task != null)
            {
                try
                {
                    await Task;
                }
                catch (Exception)
                {
                    _logger.LogError("Error when stopping the service");
                }
            }
            _informations.IsWorking = false;
            _informations.ClosedTime = DateTime.Now;
            _logger.LogWarning("Close Connection");
            await base.StopAsync(cancellationToken);
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _scope = _serviceScopeFactory.CreateScope();
                NotificationDbContext = _scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
                NotificationDecorator = _scope.ServiceProvider.GetRequiredService<INotificationDecorator>();

                if (NotificationDbContext == null || NotificationDecorator == null)
                {
                    throw new ServiceNullReferenceException("NotificationDbContext is null");
                }
                HandlerStopToken = stoppingToken;
                Task = Task.Run(async () =>
                {
                    try
                    {
                        await InitListener(HandlerStopToken);
                    }

                    catch (OperationCanceledException)
                    {
                        _logger.LogCritical("Operation was canceled, stopping the service.");
                        throw;
                    }
                    catch (TooManyRetriesException err)
                    {
                        _logger.LogCritical("Too many retries, stopping the service.", err);
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("Error in listener",ex);
                        throw;
                    }
                }, stoppingToken);

            }
            catch (ServiceNullReferenceException err)
            {
                _logger.LogError(err,"No service");
                throw;
            }
            catch (Exception err)
            {
                _logger.LogError(err,"Error" );
                throw;
            }

            return Task.FromResult(true);
        }

        private Task InitListener(CancellationToken stoppingToken)
        {
            var queueName = _rabbitOptionsExtended?.ListenerQueues?.Where(pr => pr.Name == "Notification").FirstOrDefault()?.Name;
            if(_rabbitOptionsExtended == null || string.IsNullOrEmpty(queueName))
            {
                throw new ServiceNullReferenceException("Rabbit options or queue name is null");
            }

            return _rabbitListenerService.InitListenerQueueAsync(_rabbitOptionsExtended, queueName , async (RabbitMessageModel rabbit) =>
            {
                NotificationMessageModel notificationMessageModel = JsonSerializer.Deserialize<NotificationMessageModel>(rabbit.Message) ?? throw new Exception("Failed to deserialize message");
                RabbitMessageModel message = new()
                {
                    Message = notificationMessageModel.Message,
                };
                //Get user notification settings
                NotificationSettings notificationSettings;

                //If force notification is set, use dummy settings
                if (notificationMessageModel.ForceNotification.IsAnyForce())
                {
                    notificationSettings = NotificationSettingsExtensions.DummySettings(notificationMessageModel.ForceNotification, notificationMessageModel.AccountId);
                }
                //Otherwise, get from database
                else
                {
                    notificationSettings = await NotificationDbContext!.NotificationSettings.Where(pr => pr.AccountId == notificationMessageModel.AccountId).FirstOrDefaultAsync(stoppingToken)
                        ?? throw new NotificationSettingArgumentNullException("User notification settings not found for accountId: " + notificationMessageModel.AccountId);
                }

                await HandleDecorator(message, notificationSettings, stoppingToken);
            }, stoppingToken);
        }

        private Task<bool> HandleDecorator(RabbitMessageModel parameters, NotificationSettings userNotificationSettings, CancellationToken cancellationToken)
        {
            if(NotificationDecorator == null)
            {
                throw new ServiceNullReferenceException("NotificationDecorator is null");
            }
            return NotificationDecorator.HandleNotification(parameters, userNotificationSettings, cancellationToken);
        }
    }
}
