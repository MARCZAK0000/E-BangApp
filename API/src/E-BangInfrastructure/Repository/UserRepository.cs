using E_BangDomain.Entities;
using E_BangDomain.Repository;
using E_BangInfrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace E_BangInfrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ProjectDbContext _projectDbContext;
        public UserRepository(ProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }
        public async Task<Users?> GetUserByAccountId(string accountId, CancellationToken token) =>
            await _projectDbContext
                .Users
                .Where(u => u.UserID == accountId)
                .FirstOrDefaultAsync(token);

    }
}
