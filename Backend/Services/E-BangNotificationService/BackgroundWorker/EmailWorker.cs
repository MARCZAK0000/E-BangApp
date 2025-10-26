using BackgrounMessageQueues;
using CustomLogger.Abstraction;
using FactoryPattern;
using StrategyPattern;

namespace BackgroundWorker
{
    public class EmailWorker : BackgroundService
    {
        private readonly ICustomLogger<SmsWorker> _logger;

        private readonly IQueueHandlerStrategy _queueHandlerStrategy;

        private IQueueHandlerService? _queueHandlerService;
        public EmailWorker(ICustomLogger<SmsWorker> logger, IQueueHandlerStrategy queueHandlerStrategy)
        {
            _logger = logger;
            _queueHandlerStrategy = queueHandlerStrategy;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EmailWorker Service is starting.", DateTime.Now);
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("EmailWorker Service is stopping.", DateTime.Now);
            return base.StopAsync(cancellationToken); 
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
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                        // Obsługa anulowania operacji, gdy usługa jest zatrzymywana
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "An error occurred while processing a work item in EmailWorker");
                        if (System.Diagnostics.Debugger.IsAttached)
                        {
                            System.Diagnostics.Debugger.Break();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in EmailWorker at {datetime}: {ex}, {ex.endpoint}");
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
            finally
            {
                _queueHandlerService?.Dispose();
            }
        }
    }
}
