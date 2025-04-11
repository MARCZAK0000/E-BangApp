using E_BangAppRabbitSharedClass.RabbitModel;
using E_BangEmailWorker.Database;

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

        public Task SaveEmailInfo(EmailServiceRabbitMessageModel emailServiceRabbitMessageModel, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
