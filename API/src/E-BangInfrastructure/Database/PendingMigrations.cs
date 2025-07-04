using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace E_BangInfrastructure.Database
{
    public class PendingMigrations
    {
        private readonly ProjectDbContext _context;

        private readonly ILogger<PendingMigrations> _logger;

        public PendingMigrations(ProjectDbContext context, ILogger<PendingMigrations> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void GetPendingMigrations()
        {
            _logger.LogInformation("{nameof} at {date} : Connect To Database", nameof(PendingMigrations), DateTime.Now);
            if (_context.Database.CanConnect())
            {
                _logger.LogInformation("{nameof} at {date} : Get Pending Migrations", nameof(PendingMigrations), DateTime.Now);
                _context.Database.GetPendingMigrations();
            }
        }
    }
}
