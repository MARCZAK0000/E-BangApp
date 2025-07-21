using E_BangDomain.Entities;
using Microsoft.Extensions.Logging;
using System;

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
                    _context.Roles.AddRange(
                        new Roles { RoleName = "Admin", RoleDescription = "ADMIN", RoleLevel = 5 },
                        new Roles { RoleName = "User", RoleDescription = "USER", RoleLevel = 0 },
                        new Roles { RoleName = "Owner", RoleDescription = "OWNER", RoleLevel = 3 },
                        new Roles { RoleName = "ShopStaff", RoleDescription = "SHOPSTAFF", RoleLevel = 1 },
                        new Roles { RoleName = "ShopModerator", RoleDescription = "SHOPMODERATOR", RoleLevel = 2 },
                        new Roles { RoleName = "Moderator", RoleDescription = "MODERATOR", RoleLevel = 4 }
                    );
                }
                if (!_context.Actions.Any())
                {
                    _context.Actions.AddRange(
                        new Actions { ActionName = "Create", ActionDescription = "Create Action" },
                        new Actions { ActionName = "Read", ActionDescription = "Read Action" },
                        new Actions { ActionName = "Update", ActionDescription = "Update Action" },
                        new Actions { ActionName = "Delete", ActionDescription = "Delete Action" }
                    );
                }
                if (!_context.ActionInRoles.Any())
                {
                    List<Roles> roles = _context.Roles.ToList();
                    List<Actions> actions = _context.Actions.ToList();

                    foreach (var role in roles)
                    {
                        switch (role.RoleLevel)
                        {
                            case 0:
                            default:// User
                                actions.Where(action => action.ActionName == "Read").ToList().ForEach(action =>
                                {
                                    _context.ActionInRoles.Add(new ActionInRole
                                    {
                                        ActionID = action.ActionID,
                                        RoleID = role.RoleID,
                                        LastUpdateTime = DateTime.Now
                                    });
                                });
                                break;
                            case 1: // ShopStaff
                                actions.Where(action => action.ActionName == "Read" || action.ActionName == "Update").ToList().ForEach(action =>
                                {
                                    _context.ActionInRoles.Add(new ActionInRole
                                    {
                                        ActionID = action.ActionID,
                                        RoleID = role.RoleID,
                                        LastUpdateTime = DateTime.Now
                                    });
                                });
                                break;
                            case 2: // ShopModerator
                                actions.Where(action => action.ActionName == "Read" || action.ActionName == "Update" || action.ActionName == "Create").ToList().ForEach(action =>
                                {
                                    _context.ActionInRoles.Add(new ActionInRole
                                    {
                                        ActionID = action.ActionID,
                                        RoleID = role.RoleID,
                                        LastUpdateTime = DateTime.Now
                                    });
                                });
                                break;
                            case 3:
                            case 4: // Moderator and Owner
                                actions.ForEach(action =>
                                {
                                    _context.ActionInRoles.Add(new ActionInRole
                                    {
                                        ActionID = action.ActionID,
                                        RoleID = role.RoleID,
                                        LastUpdateTime = DateTime.Now
                                    });
                                });
                                break;
                        } ;
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
}
