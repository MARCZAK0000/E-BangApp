using E_BangEmailWorker.Services;
using Microsoft.Extensions.DependencyInjection;

namespace E_BangEmailWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    private IDatabaseService _databaseService;
    public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _databaseService = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IDatabaseService>();
        while (!stoppingToken.IsCancellationRequested)
        {

            if (_logger.IsEnabled(LogLevel.Information))
            {
                var emails = await _databaseService.CurrentEmailsInQueueAsync(stoppingToken);
                if (emails.Count > 0)
                {
                    _logger.LogInformation(emails.Count.ToString());
                }
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(1000 * 60, stoppingToken);
        }
    }
}
