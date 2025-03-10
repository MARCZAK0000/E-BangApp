using E_BangEmailWorker.Services;
using Microsoft.Extensions.DependencyInjection;

namespace E_BangEmailWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    private IDatabaseService _databaseService;
    
    private IEmailServices _emailServices;
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
            _databaseService = scope.ServiceProvider.GetRequiredService<IDatabaseService>();
            _emailServices = scope.ServiceProvider.GetRequiredService<IEmailServices>();
            IList<int> sendIds = new List<int>();
            _logger.LogCritical("Service worker initialized");
            while (!stoppingToken.IsCancellationRequested)
            {
                var currentEmail = await _databaseService.CurrentEmailsInQueueAsync(stoppingToken);
                if (currentEmail.Count > 0)
                {
                    foreach (var item in currentEmail)
                    {
                        if (!await _emailServices.SendMailAsync(item, stoppingToken))
                        {
                            _logger.LogError($"Email weren't send:\r\nInformations: \r\nTo: {item.AddressTo}\r\n");
                        }
                        else
                        {
                            sendIds.Add(item.Id);
                        }
                    }
                    await _databaseService.SetIsSendAsync(sendIds, stoppingToken);
                    await _databaseService.ClearEmailQueueAsync();
                }
                await Task.Delay(1000 * 60, stoppingToken);
            }
        }
        catch (Exception err)
        {
            _logger.LogError("Unexptected error: {0}", err.Message);
        }
        
    }
}
