using E_BangEmailWorker.Services;

namespace E_BangEmailWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;
    public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            IRabbitQueueService rabbitQueue = scope.ServiceProvider.GetRequiredService<IRabbitQueueService>();
            await rabbitQueue.HandleRabbitQueueAsync(stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception err)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
            _logger.LogError("Unexptected error in worker: {0}", err.Message);
        }
    }
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker initalized at {Date}", DateTime.Now);
        return base.StartAsync(cancellationToken);
    }
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Stop {DateTime.Now}");
        return base.StopAsync(cancellationToken);
    }
}
