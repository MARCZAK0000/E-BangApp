using E_BangEmailWorker.Database;
using Microsoft.EntityFrameworkCore;

namespace E_BangEmailWorker.Seeder
{
    public class GeneratorDatabaseSeed
    {
        private readonly GeneratorDbContext _context;

        public GeneratorDatabaseSeed(GeneratorDbContext context)
        {
            _context = context;
        }

        public void Migrate()
        {
            _context.Database.Migrate();
        }
    }
}
