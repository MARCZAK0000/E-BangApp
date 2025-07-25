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
                .Include(u => u.Address)
                .FirstOrDefaultAsync(token);

        public async Task<bool> UpdateUserAsync(Users user, CancellationToken token)
        {
            _projectDbContext
                .Users
                .Update(user);  

            return await _projectDbContext
                .SaveChangesAsync(token) > 0;
        }
    }
}
