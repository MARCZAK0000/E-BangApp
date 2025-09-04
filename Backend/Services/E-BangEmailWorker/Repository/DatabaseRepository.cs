using E_BangAppRabbitSharedClass.BuildersDto.RabbitMessageChilds;
using E_BangAppRabbitSharedClass.RabbitModel;
using E_BangEmailWorker.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace E_BangEmailWorker.Repository
{
    public class DatabaseRepository : IDatabaseRepository
    {
        private readonly ServiceDbContext _context;
        private readonly ILogger<DatabaseRepository> _logger;
        public DatabaseRepository(ServiceDbContext context, ILogger<DatabaseRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SaveEmailInfo(EmailBody emailBody, CancellationToken token)
        {
            
            using IDbContextTransaction dbContextTransaction 
                = await _context.Database.BeginTransactionAsync(token);
            try
            {
                await _context.Emails.AddAsync(new Email
                {
                    EmailAddress = emailBody.AdressedTo,
                    CreatedTime = DateTime.UtcNow,
                    IsSend = true,
                    SendTime = DateTime.UtcNow,
                }, token);
                await dbContextTransaction.CommitAsync(token);
            }
            catch (Exception ex)
            {
                await dbContextTransaction.RollbackAsync(token);
                _logger.LogError("Error in transaction: {ex}", ex.Message);
                throw;
            }
        }
    }
}
