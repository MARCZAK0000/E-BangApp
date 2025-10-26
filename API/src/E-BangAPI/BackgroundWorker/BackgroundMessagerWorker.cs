using App.RabbitBuilder.Exceptions;
using CustomLogger.Abstraction;
using E_BangDomain.IQueueService;

namespace E_BangAPI.BackgroundWorker
{
    public class BackgroundMessagerWorker : BackgroundService
    {
        private readonly IMessageSenderHandlerQueue _messageHandlerQueueService;
        private readonly ICustomLogger<BackgroundMessagerWorker> _logger;
        public BackgroundMessagerWorker(IMessageSenderHandlerQueue messageHandlerQueueService, ICustomLogger<BackgroundMessagerWorker> logger)
        {
            _messageHandlerQueueService = messageHandlerQueueService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(500, stoppingToken);
                var workitem = await _messageHandlerQueueService.DequeueAsync(stoppingToken);
                try
                {
                    await workitem(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Background Messager Worker: Operation cancelled");
                }
                catch (TooManyRetriesException ex)
                {
                    _logger.LogError(ex, "Background Messager Worker: Too many retries - {ErrorMessage}", ex.Message);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Background Messager Worker: Error occurred executing work item - {ErrorMessage}", ex.Message);
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        System.Diagnostics.Debugger.Break();
                    }
                }
            }
        }
    }
}
