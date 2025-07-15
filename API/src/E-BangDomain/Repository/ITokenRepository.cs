using E_BangDomain.Entities;

namespace E_BangDomain.Repository
{
    public interface ITokenRepository
    {
        string GenerateJWTTokenAsync(Account account);
        string GenerateTwoWayFactoryToken();
    }
}
