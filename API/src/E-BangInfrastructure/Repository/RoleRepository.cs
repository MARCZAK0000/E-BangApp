using E_BangDomain.Entities;
using E_BangDomain.Repository;
using E_BangInfrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
           await _projectDbContext
                .UsersInRoles
                .AddAsync(new UsersInRole
                {
                    UserID = accountId,
                    RoleID = roleID
                }, token);

            return await _projectDbContext.SaveChangesAsync(token) > 0;
        }

        public async Task<List<string>> GetRoleNamesByAccountIdAsync(string accountId, CancellationToken token) 
            => await _projectDbContext
                .UsersInRoles
                .Where(x => x.UserID == accountId)
                .Include(x => x.Roles)
                .OrderBy(o=>o.Roles.RoleLevel)
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
            await _projectDbContext
                .UsersInRoles
                .AddAsync(new UsersInRole
                {
                    UserID = accountId,
                    RoleID = roleID  // Assuming "0" is the ID for Role Level Zero
                }, token);

            return await _projectDbContext.SaveChangesAsync(token) > 0;
        }

        public async Task<List<Roles>> GetRolesByAccountIdAsync(string accountId, CancellationToken token)
        {
            return await _projectDbContext
                .UsersInRoles
                .Where(pr=>pr.UserID == accountId)
                .Select(pr=>pr.Roles)
                .ToListAsync(token);
        }

        public async Task<bool> AddRoleAsync(Roles roles, CancellationToken token)
        {
            await _projectDbContext.Roles.AddAsync(roles, token);
            return await _projectDbContext.SaveChangesAsync(token) > 0;
        }

        public async Task<bool> UpdateRoleLevelAsync(int minRoleLevel, CancellationToken token)
        {
            int rowCount = await _projectDbContext.Roles.Where(pr => pr.RoleLevel >= minRoleLevel)
                .ExecuteUpdateAsync(sp => sp.SetProperty(p => p.RoleLevel, p=>p.RoleLevel+1),token);

            return rowCount > 0;
        }
    }
}
