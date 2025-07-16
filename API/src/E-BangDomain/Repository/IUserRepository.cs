using E_BangDomain.Entities;

namespace E_BangDomain.Repository
{
    public interface IUserRepository
    {
        public Task<Users?> GetUserByAccountId(string accountId, CancellationToken token);
    }
}
