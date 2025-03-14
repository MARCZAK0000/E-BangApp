namespace E_BangAzureWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            
        }
    }
    public override void Dispose()
    {
        base.Dispose();
    }
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        return base.StartAsync(cancellationToken);
    }
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        return base.StopAsync(cancellationToken);
    }
}
