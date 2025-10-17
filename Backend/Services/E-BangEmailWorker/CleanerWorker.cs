
using E_BangEmailWorker.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace E_BangEmailWorker
{
    public class CleanerWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IServiceScope? _scope; 
        private readonly ILogger<CleanerWorker> _logger;    

        public CleanerWorker(IServiceScopeFactory serviceScopeFactory,
            ILogger<CleanerWorker> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _scope = _serviceScopeFactory.CreateScope();
            var dbContext = _scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    SqlParameter sqlParameter = new SqlParameter("@Date", System.Data.SqlDbType.DateTimeOffset)
                    {
                        Value = DateTimeOffset.Now.AddDays(-30)
                    };
                    dbContext.Database.ExecuteSqlRaw("EXEC Maintenance.RemoveOldEmails @Date", [sqlParameter]);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while cleaning old emails.");
                }
                Task.Delay(TimeSpan.FromHours(24), stoppingToken).Wait(stoppingToken);
            }
            return Task.CompletedTask;
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _scope?.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
