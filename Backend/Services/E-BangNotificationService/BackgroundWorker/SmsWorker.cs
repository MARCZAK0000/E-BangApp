namespace BackgroundWorker
{
    public class SmsWorker : BackgroundService
    {
        private readonly ILogger<SmsWorker> _logger;
        public SmsWorker(ILogger<SmsWorker> logger)
        {
            _logger = logger;
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Date}: SmsWorker Service is starting.", DateTime.Now);
            return base.StartAsync(cancellationToken);
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("{Date}: SmsWorker Service is stopping.", DateTime.Now);
            return base.StopAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    // Implement SMS sending logic here
                    await Task.Delay(1000, stoppingToken); // Delay to prevent tight loop
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in SmsWorker");
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    System.Diagnostics.Debugger.Break();
                }
            }
        }
    }
}

