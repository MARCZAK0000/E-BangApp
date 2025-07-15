using E_BangDomain.Entities;

namespace E_BangDomain.Repository
{
    public interface ITokenRepository
    {
        public string GenerateJWTTokenAsync();
        public string GenerateTwoWayFactoryToken();
        public bool SaveCookiesAsync(string jwtToken);
        
    }
}
