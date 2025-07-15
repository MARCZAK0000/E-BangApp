using E_BangDomain.Entities;
using E_BangDomain.Repository;

namespace E_BangInfrastructure.Repository
{
    public class TokenRepository : ITokenRepository
    {
        public string GenerateJWTTokenAsync()
        {
            throw new NotImplementedException();
        }

        public string GenerateTwoWayFactoryToken()
        {
            throw new NotImplementedException();
        }

        public bool SaveCookiesAsync(string jwtToken)
        {
            throw new NotImplementedException();
        }
    }
}
