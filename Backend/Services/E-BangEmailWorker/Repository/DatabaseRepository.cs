using CustomLogger.Abstraction;
using E_BangEmailWorker.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace E_BangEmailWorker.Repository
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly ServiceDbContext _context;
        private readonly ICustomLogger<DatabaseRepository> _logger;
        public DatabaseRepository(ServiceDbContext context, ICustomLogger<DatabaseRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SaveEmailInfo(string address, bool isSend, CancellationToken token)
        {

            using IDbContextTransaction dbContextTransaction
                = await _context.Database.BeginTransactionAsync(token);
            try
            {
                await _context.Emails.AddAsync(new Email
                {
                    EmailAddress = address,
                    CreatedTime = DateTime.UtcNow,
                    IsSend = isSend,
                    SendTime = DateTime.UtcNow,
                }, token);
                await dbContextTransaction.CommitAsync(token);
            }
            catch (Exception ex)
            {
                await dbContextTransaction.RollbackAsync(token);
                _logger.LogError(ex, "Error in transaction: {ex}", ex.Message);
                throw;
            }
        }
    }
}
