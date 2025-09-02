using E_BangAppRabbitBuilder.Exceptions;
using E_BangEmailWorker.Services;

namespace E_BangEmailWorker;

public class Worker : BackgroundService
{
    private IRabbitQueueService? rabbitQueue;
    private readonly ILogger<Worker> _logger;
    private IServiceScope? scope;
    private Task? _listenerTask;
    private CancellationToken handlerStopToken;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        handlerStopToken = stoppingToken;
        scope = _serviceScopeFactory.CreateScope();
        rabbitQueue = scope.ServiceProvider.GetRequiredService<IRabbitQueueService>();
        _listenerTask = Task.Run(async () =>
        {
            try
            {
                await InitializeQueueAsync(handlerStopToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Operation was canceled, stopping the service.");
                throw;
            }
            catch (TooManyRetriesException err)
            {
                _logger.LogError(err, "Too many retries, stopping the service.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in listener");
                throw;
            }
        }, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    private async Task InitializeQueueAsync(CancellationToken cancellationToken)
    {
        await rabbitQueue!.HandleRabbitQueueAsync(cancellationToken);
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker initalized at {Date}", DateTime.Now);
        return base.StartAsync(cancellationToken);
    }
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if(_listenerTask != null )
        {
            try
            {
                await _listenerTask;
            }
            catch (Exception)
            {
                _logger.LogError("Error in listener task during stopping.");
                throw;
            }
        }   
        scope?.Dispose();
        _logger.LogInformation($"Stop {DateTime.Now}");
        await base.StopAsync(cancellationToken);
    }
}
