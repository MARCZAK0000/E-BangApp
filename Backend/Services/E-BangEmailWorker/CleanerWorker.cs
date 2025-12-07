
using CustomLogger.Abstraction;
using E_BangEmailWorker.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace E_BangEmailWorker
{
    public class CleanerWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IServiceScope? _scope; 
        private readonly ICustomLogger<CleanerWorker> _logger;    

        public CleanerWorker(IServiceScopeFactory serviceScopeFactory,
            ICustomLogger<CleanerWorker> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _scope = _serviceScopeFactory.CreateScope();
            var dbContext = _scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    SqlParameter rowCount = new SqlParameter("@UpdatedRows", System.Data.SqlDbType.Int)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    SqlParameter date = new SqlParameter("@Date", System.Data.SqlDbType.DateTimeOffset)
                    {
                        Value = DateTimeOffset.Now.AddDays(-30)
                    };
                    SqlParameter resultMessage = new SqlParameter("@ResultMessage", System.Data.SqlDbType.NVarChar, 100)
                    {
                        Direction = System.Data.ParameterDirection.Output
                    };
                    int result = dbContext.Database.ExecuteSqlRaw("EXEC Maintenance.RemoveOldEmails @Date, @UpdatedRows OUTPUT, @ResultMessage OUTPUT", [date, rowCount, resultMessage]);
                    if(result == 0)
                    {
                        _logger.LogError("Stored Procedure execution failed.");
                    }
                    else
                    {
                        if (rowCount.Value != null && resultMessage.Value != null &&
                            rowCount.Value != DBNull.Value && resultMessage.Value != DBNull.Value)
                        {
                            int deletedRows = (int)rowCount.Value;
                            string? resultMsg = resultMessage.Value.ToString();
                            _logger.LogInformation("Deleted {0} old emails. \r\n{1}", deletedRows, resultMsg ?? string.Empty);
                        }
                        else
                        {
                            _logger.LogCritical("OUTPUTs are null, Check Procedure!!");
                        }
                    }
                }
                catch (InvalidCastException ex)
                {
                    _logger.LogError(ex,"Invalid cast");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while cleaning old emails.");
                }
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _scope?.Dispose();
            return base.StopAsync(cancellationToken);
        }
    }
}
