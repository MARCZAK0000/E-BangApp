using BackgrounMessageQueues;
using FactoryPattern;
using StrategyPattern;

namespace BackgroundWorker
{
    public class EmailWorker : BackgroundService
    {
        private readonly ILogger<SmsWorker> _logger;

        private readonly IQueueHandlerStrategy _queueHandlerStrategy;

        private IQueueHandlerService? _queueHandlerService;
        public EmailWorker(ILogger<SmsWorker> logger, IQueueHandlerStrategy queueHandlerStrategy)
        {
            _logger = logger;
            _queueHandlerStrategy = queueHandlerStrategy;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Date}: EmailWorker Service is starting.", DateTime.Now);
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Date}: EmailWorker Service is stopping.", DateTime.Now);
            return base.StopAsync(cancellationToken); // To zatrzyma ExecuteAsync przez CancellationToken
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _queueHandlerService = await _queueHandlerStrategy.HandleQueueAsync(EQueue.Email);
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(200), stoppingToken);
                    try
                    {
                        var workItem = await _queueHandlerService.DequeueAsync(stoppingToken);
                        await workItem(stoppingToken);
                    }
                    catch (OperationCanceledException)
                    {
                        break;
                    }
                }
            }
            finally
            {
                _queueHandlerService?.Dispose();
            }
        }
    }
}
