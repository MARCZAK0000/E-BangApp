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
        public async Task<List<string>> GetRoleByAccountIdAsync(string accountId, CancellationToken token)
        {
            return await _projectDbContext
                .UsersInRoles
                .Where(x=>x.UserID == accountId)
                .Include(x=>x.Roles)
                .Select(x => x.Roles.RoleName)
                .ToListAsync(token);
        }
    }
}
