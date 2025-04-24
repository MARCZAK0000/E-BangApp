using E_BangDomain.IQueueService;

namespace E_BangAPI.BackgroundWorker
{
    public class BackgroundMessagerWorker : BackgroundService
    {
        private readonly IMessageSenderHandlerQueue _messageHandlerQueueService;
        public BackgroundMessagerWorker(IMessageSenderHandlerQueue messageHandlerQueueService)
        {
            _messageHandlerQueueService = messageHandlerQueueService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workitem = await _messageHandlerQueueService.DequeueAsync(stoppingToken);
                try
                {
                    await workitem(stoppingToken);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}
