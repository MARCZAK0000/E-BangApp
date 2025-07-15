using E_BangDomain.Entities;

namespace E_BangDomain.Repository
{
    public interface ITokenRepository
    {
        string GenerateJWTTokenAsync();

        string GenerateTwoWayFactoryToken();

        bool SaveCookiesAsync(string jwtToken);
    }
}
