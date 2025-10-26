using CustomLogger.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace E_BangInfrastructure.Database
{
    public class PendingMigrations
    {
        private readonly ProjectDbContext _context;

        private readonly ICustomLogger<PendingMigrations> _logger;

        public PendingMigrations(ProjectDbContext context, ICustomLogger<PendingMigrations> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void GetPendingMigrations()
        {
            _logger.LogInformation("Connect To Database: {Name}", _context.Database.ProviderName ?? "Unkown");
            if (_context.Database.CanConnect())
            {
                _logger.LogInformation("Get Pending Migrations");
                _context.Database.Migrate();
            }
        }
    }
}
