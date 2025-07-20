using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace E_BangInfrastructure.Database
{
    public class Seeder
    {
        private readonly ILogger<Seeder> _logger;

        private readonly ProjectDbContext _context;

        public Seeder(ProjectDbContext context, ILogger<Seeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void SeedDb()
        {
            _logger.LogInformation("{nameof} at {date} : Connect To Database", nameof(Seeder), DateTime.Now);
            if (_context.Database.CanConnect())
            {
                _logger.LogInformation("{nameof} at {date} : Seeding Database", nameof(Seeder), DateTime.Now);
                _context.Database.EnsureCreated();
                // Add seed data here if necessary
                if (!_context.Roles.Any())
                {
                    _context.Database.ExecuteSqlRaw("");
                }
                _context.SaveChanges();
            }
            else
            {
                _logger.LogError("{nameof} at {date} : Failed to connect to database", nameof(Seeder), DateTime.Now);
            }
        }
    }
}
