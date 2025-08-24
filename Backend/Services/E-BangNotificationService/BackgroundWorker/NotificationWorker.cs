using E_BangNotificationService.AppInfo;
using E_BangNotificationService.Service;

namespace E_BangNotificationService.BackgroundWorker
{
    public class NotificationWorker : BackgroundService
    {
        private readonly IInformations _informations;

        private readonly ILogger<NotificationWorker> _logger;

        private readonly IServiceScopeFactory _serviceScopeFactory;

        private IServiceScope? scope;
        public NotificationWorker(IInformations informations,
            ILogger<NotificationWorker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _informations = informations;
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _informations.InitTime = DateTime.Now;
            _informations.IsWorking = true;
            _logger.LogInformation("{Date}: Init Connection", DateTime.Now);
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _informations.IsWorking = false;
            _informations.ClosedTime = DateTime.Now;
            scope?.Dispose();
            _logger.LogInformation("{Date}: Close Connection", DateTime.Now);
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                scope = _serviceScopeFactory.CreateScope();
                IRabbitMQService rabbitMQService = scope.ServiceProvider.GetRequiredService<IRabbitMQService>();
                await rabbitMQService.CreateListenerQueueAsync(stoppingToken);

                while (!stoppingToken.IsCancellationRequested)
                {
                    _informations.CurrentTime = DateTime.Now;
                    _informations.IsWorking = true;
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Something unexpected happens at {datetime}: {ex}, {ex.endpoint}", DateTime.Now, ex.Message, ex.Source);
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }

        }
        
    }
}
