using E_BangDomain.Entities;
using E_BangDomain.Repository;
using E_BangInfrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace E_BangInfrastructure.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ProjectDbContext _projectDbContext;
        public RoleRepository(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<bool> AddToRoleAsync(string accountId, string roleID, CancellationToken token)
        {
           var result = await _projectDbContext
                .UsersInRoles
                .AddAsync(new UsersInRole
                {
                    UserID = accountId,
                    RoleID = roleID
                }, token);
            
            return result is not null && result.Entity is not null;
        }

        public async Task<List<string>> GetRoleByAccountIdAsync(string accountId, CancellationToken token) 
            => await _projectDbContext
                .UsersInRoles
                .Where(x => x.UserID == accountId)
                .Include(x => x.Roles)
                .Select(x => x.Roles.RoleName)
                .ToListAsync(token);

        public async Task<List<Roles>> GetRolesAsync(CancellationToken token)
            => await _projectDbContext
                .Roles
                .ToListAsync(token);

        public async Task<bool> AddToRoleLevelZeroAsync(string accountId, CancellationToken token)
        {
            string? roleID = await _projectDbContext
                .Roles
                .Where(x => x.RoleLevel == 0)
                .Select(s => s.RoleID)
                .FirstOrDefaultAsync(token);

            if(string.IsNullOrEmpty(roleID))
            {
                return false; // Role Level Zero not found
            }
            var result = await _projectDbContext
                .UsersInRoles
                .AddAsync(new UsersInRole
                {
                    UserID = accountId,
                    RoleID = roleID  // Assuming "0" is the ID for Role Level Zero
                }, token);

            return result is not null && result.Entity is not null;
        }
    }
}
