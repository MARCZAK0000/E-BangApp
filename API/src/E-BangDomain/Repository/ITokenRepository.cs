using E_BangDomain.Entities;
using E_BangDomain.RequestDtos.TokenRepostitoryDtos;
using E_BangDomain.Settings;
using System.Security.Claims;

namespace E_BangDomain.Repository
{
    public interface ITokenRepository
    {
        List<Claim> GenerateClaimsList (Account account, List<string> roles);
        string GenerateToken(List<Claim> claims);
        string GenerateTwoWayFactoryToken();
        public string GenerateRefreshToken();
        bool SaveCookies(List<SaveCookiesDtos> cookiesDtos);
    }
}
