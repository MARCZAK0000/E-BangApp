using E_BangAzureWorker.EventPublisher;
using E_BangAzureWorker.Services;

namespace E_BangAzureWorker;

public class Worker : BackgroundService
{
    private IRabbitMQService? _rabbitMQService;
    private IEventPublisher? _eventPublisher;
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
            using var scope = _serviceScopeFactory.CreateScope();
            _eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();
            _rabbitMQService = scope.ServiceProvider.GetRequiredService<IRabbitMQService>();
            await _rabbitMQService.HandleReciverQueueAsync(stoppingToken);

            _eventPublisher!.ReceivedMessageAsync += async (sender, obj) =>
            {
                await _rabbitMQService.HandleSendQueueAsync(obj, stoppingToken);
            };
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        }
        catch (Exception err)
        {
            _logger.LogError("Email Worker: Error ocured at {Date} in Worker: {ex}", DateTime.Now, err.Message);
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

    }
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Email Worker: Worker Initailized at {DateTime}", DateTime.Now);
        return base.StartAsync(cancellationToken);
    }
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Email Worker: Worker Closed at {DateTime}", DateTime.Now);
        return base.StopAsync(cancellationToken);
    }
}
