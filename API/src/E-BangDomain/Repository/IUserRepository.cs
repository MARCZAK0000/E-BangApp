using E_BangDomain.Entities;

namespace E_BangDomain.Repository
{
    public interface IUserRepository
    {
        public Task<Users?> GetUserByAccountId(string accountId, CancellationToken token);

        Task<bool> UpdateUserAsync(Users user, CancellationToken token);

        Task<bool> AddUserAsync(Users user, CancellationToken token);
    }
}
