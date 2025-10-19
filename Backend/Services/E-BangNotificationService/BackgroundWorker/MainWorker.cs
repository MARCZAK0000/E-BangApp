using App.RabbitBuilder.Exceptions;
using App.RabbitBuilder.Options;
using App.RabbitBuilder.Service.Listener;
using App.RabbitSharedClass.Notifications;
using App.RabbitSharedClass.UniversalModel;
using AppInfo;
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

        private readonly ILogger<MainWorker> _logger;

        private readonly IRabbitListenerService _rabbitListenerService;

        private readonly RabbitOptionsExtended _rabbitOptionsExtended;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private NotificationDbContext? NotificationDbContext;

        private CancellationToken HandlerStopToken;

        private IServiceScope? _scope;

        private Task? Task;

        private INotificationDecorator? NotificationDecorator;

        public MainWorker(IInformations informations,
            ILogger<MainWorker> logger,
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
            _logger.LogInformation("{Date}: Init Connection", DateTime.Now);
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
            _logger.LogInformation("{Date}: Close Connection", DateTime.Now);
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
                        _logger.LogInformation("Operation was canceled, stopping the service.");
                        throw;
                    }
                    catch (TooManyRetriesException err)
                    {
                        _logger.LogError(err, "Too many retries, stopping the service.");
                        throw;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error in listener");
                        throw;
                    }
                }, stoppingToken);

            }
            catch (ServiceNullReferenceException err)
            {
                _logger.LogError(err, "ServiceNullReferenceException: {message} - {trace}", err.Message, err.StackTrace);
                throw;
            }
            catch (Exception err)
            {
                _logger.LogError(err, "Error when executing the service: {message} - {trace}", err.Message, err.StackTrace);
                throw;
            }

            return Task.FromResult(true);
        }

        private Task InitListener(CancellationToken stoppingToken)
        {
            return _rabbitListenerService.InitListenerQueueAsync(_rabbitOptionsExtended, _rabbitOptionsExtended.ListenerQueues.Where(pr=>pr.Name == "Notification").FirstOrDefault().Name , async (RabbitMessageModel rabbit) =>
            {
                NotificationMessageModel notificationMessageModel = JsonSerializer.Deserialize<NotificationMessageModel>(rabbit.Message) ?? throw new Exception("Failed to deserialize message");
                RabbitMessageModel message = new()
                {
                    Message = notificationMessageModel.Message,
                };

                if (notificationMessageModel.ForceEmail || notificationMessageModel.ForceNotification || notificationMessageModel.ForceSms)
                {
                    /// Create a dummy NotificationSettings object based on the flags in UniMessageModel
                    NotificationSettings dummyNotificationSettings = new NotificationSettings
                    {
                        IsEmailNotificationEnabled = notificationMessageModel.ForceEmail,
                        IsPushNotificationEnabled = notificationMessageModel.ForceNotification,
                        IsSmsNotificationEnabled = notificationMessageModel.ForceSms,
                        AccountId = notificationMessageModel.AccountId,
                        LastUpdated = DateTime.Now
                    };

                    await HandleDecorator(message, dummyNotificationSettings, stoppingToken);
                    return;
                }
                NotificationSettings? userNotificationSettings = await NotificationDbContext!.NotificationSettings.Where(pr => pr.AccountId == notificationMessageModel.AccountId).FirstOrDefaultAsync(stoppingToken);
                if (userNotificationSettings is null)
                {
                    throw new NotificationSettingArgumentNullException("User notification settings not found for accountId: " + notificationMessageModel.AccountId);
                }
                await HandleDecorator(message, userNotificationSettings, stoppingToken);
            }, stoppingToken);
        }

        private Task HandleDecorator(RabbitMessageModel parameters, NotificationSettings userNotificationSettings, CancellationToken cancellationToken)
        {
            return NotificationDecorator!.HandleNotification(parameters, userNotificationSettings, cancellationToken);
        }

    }
}
