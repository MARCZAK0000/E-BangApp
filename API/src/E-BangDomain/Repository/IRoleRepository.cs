namespace E_BangDomain.Repository
{
    public interface IRoleRepository
    {
        Task<List<string>> GetRoleByAccountIdAsync(string accountId, CancellationToken token);
    }
}
