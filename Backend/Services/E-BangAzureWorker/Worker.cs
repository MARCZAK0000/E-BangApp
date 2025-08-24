using E_BangAzureWorker.EventPublisher;
using E_BangAzureWorker.Services;

namespace E_BangAzureWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    private IServiceScope? _scope;
    private IEventPublisher? _eventPublisher;
    private IRabbitMQService? _rabbitMQService;
    private Task? _listenerTask;
    private CancellationToken _stoppingToken;

    public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _stoppingToken = stoppingToken;

        try
        {
            _scope = _serviceScopeFactory.CreateScope();
            _eventPublisher = _scope.ServiceProvider.GetRequiredService<IEventPublisher>();
            _rabbitMQService = _scope.ServiceProvider.GetRequiredService<IRabbitMQService>();

            _listenerTask = Task.Run(async () =>
            {
                try
                {
                    await StartListenerAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "B³¹d w listenerze RabbitMQ");
                }
            }, stoppingToken);

            _eventPublisher.ReceivedMessageAsync += OnReceivedMessageAsync;

            return Task.CompletedTask;
        }
        catch (Exception err)
        {
            _logger.LogError(err, "Email Worker: Error occurred at {Date}", DateTime.Now);
            if (System.Diagnostics.Debugger.IsAttached)
                System.Diagnostics.Debugger.Break();
        }
        return Task.CompletedTask;
    }

    private async Task OnReceivedMessageAsync(object? sender, EventMessageArgs e)
    {
        if (_rabbitMQService != null)
        {
            await _rabbitMQService.HandleSendQueueAsync(e, _stoppingToken);
        }
    }

    private async Task StartListenerAsync(CancellationToken stoppingToken)
    {
        if (_rabbitMQService != null)
        {
            await _rabbitMQService.HandleReciverQueueAsync(stoppingToken);
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Email Worker: Worker Closed at {DateTime}", DateTime.Now);

        if (_eventPublisher != null)
            _eventPublisher.ReceivedMessageAsync -= OnReceivedMessageAsync;

        if (_listenerTask != null)
        {
            try
            {
                await _listenerTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "B³¹d podczas zatrzymywania listenera RabbitMQ");
            }
        }
        _scope?.Dispose();

        await base.StopAsync(cancellationToken);
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Email Worker: Worker Initialized at {DateTime}", DateTime.Now);
        return base.StartAsync(cancellationToken);
    }
}

