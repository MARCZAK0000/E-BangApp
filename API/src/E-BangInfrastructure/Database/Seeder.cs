using E_BangDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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

        public async Task SeedDb()
        {
            _logger.LogInformation("{nameof} at {date} : Connect To Database", nameof(Seeder), DateTime.Now);
            if (_context.Database.CanConnect())
            {
                _logger.LogInformation("{nameof} at {date} : Seeding Database", nameof(Seeder), DateTime.Now);
                _context.Database.EnsureCreated();
                // Add seed data here if necessary
                if (!_context.Roles.Any())
                {
                    _context.Roles.AddRange(
                        new Roles { RoleName = "Admin", RoleDescription = "ADMIN", RoleLevel = 5 },
                        new Roles { RoleName = "User", RoleDescription = "USER", RoleLevel = 0 },
                        new Roles { RoleName = "ShopOwner", RoleDescription = "SHOPOWNER", RoleLevel = 3 },
                        new Roles { RoleName = "ShopStaff", RoleDescription = "SHOPSTAFF", RoleLevel = 1 },
                        new Roles { RoleName = "ShopModerator", RoleDescription = "SHOPMODERATOR", RoleLevel = 2 },
                        new Roles { RoleName = "Moderator", RoleDescription = "MODERATOR", RoleLevel = 4 }
                    );
                }
                if (!_context.Actions.Any())
                {
                    _context.Actions.AddRange(
                        new Actions { ActionName = "Update", ActionDescription = "Update Action", ActionLevel = 1 },
                        new Actions { ActionName = "Create", ActionDescription = "Create Action", ActionLevel = 2 },
                        new Actions { ActionName = "Delete", ActionDescription = "Delete Action", ActionLevel = 4 }
                    );
                }
                
                else
                {
                    _logger.LogError("{nameof} at {date} : Failed to connect to database", nameof(Seeder), DateTime.Now);
                }
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Actions>> GetActionsStaticData()
        {
           return await _context
                .Actions
                .OrderBy(pr=>pr.ActionLevel)
                .ToListAsync();
        }

    }
}
