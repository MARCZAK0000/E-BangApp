
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
                        _logger.LogWarning("Cleaner Worker - {Date} - Stored Procedure execution failed.", DateTime.Now);
                    }
                    else
                    {
                        if (rowCount.Value != null && resultMessage.Value != null)
                        {
                            if(rowCount.Value != DBNull.Value && resultMessage.Value != DBNull.Value)
                            {
                                int deletedRows = (int)rowCount.Value;
                                _logger.LogInformation("Cleaner Worker - {Date} - Deleted {DeletedRows} old emails. \r\n{resultMessage}", DateTime.Now, deletedRows, resultMessage.Value.ToString());
                            }
                            else
                            {
                                throw new InvalidCastException("Invalid cast from DbNull to INT - Check procedure OUTPUTs");
                            }
                        }
                        else
                        {
                            _logger.LogWarning("Cleaner Worker - {Date} - OUTPUTs are null, Check Procedure!!", DateTime.Now);
                        }
                    }
                }
                catch (InvalidCastException ex)
                {
                    _logger.LogError(ex, "Cleaner worker - {Date} - Invalid cast", DateTime.Now);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Cleaner worker - {Date} - An error occurred while cleaning old emails.", DateTime.Now);
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
