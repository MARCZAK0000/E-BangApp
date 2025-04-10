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
            IServiceScope scope = _serviceScopeFactory.CreateScope();
            IRabbitQueueService rabbitQueue = scope.ServiceProvider.GetService<RabbitQueueService>()!;
            await rabbitQueue.HandleRabbitQueue(stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception err)
        {
            _logger.LogError("Unexptected error: {0}", err.Message);
        }

    }
}
