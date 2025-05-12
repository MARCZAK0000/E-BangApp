using E_BangAzureWorker.EventPublisher;
using E_BangAzureWorker.Services;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data.SqlTypes;
using System.Text;

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
            _logger.LogError("Error ocured at {Date} in Worker: {ex}", DateTime.Now, err.Message);
            throw;
        }
        
    }
    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker Initailized at {DateTime}", DateTime.Now);
        

        return base.StartAsync(cancellationToken);
    }
    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Worker Closed at {DateTime}", DateTime.Now);
        return base.StopAsync(cancellationToken);
    }
    public override void Dispose()
    {
        _rabbitMQService?.HandleDispose();
        GC.SuppressFinalize(this);
        base.Dispose(); 
    }
}
