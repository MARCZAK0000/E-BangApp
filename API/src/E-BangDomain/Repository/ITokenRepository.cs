using E_BangDomain.Entities;
using E_BangDomain.RequestDtos.TokenRepostitoryDtos;
using System.Security.Claims;

namespace E_BangDomain.Repository
{
    public interface ITokenRepository
    {
        /// <summary>
        /// Generate list of Claims
        /// </summary>
        /// <param name="account">Account Informations</param>
        /// <param name="roles">Account Roles</param>
        /// <returns> <see cref="List{Claim}"/> Claims Lis</returns>
        List<Claim> GenerateClaimsList(Account account, List<string> roles);

        /// <summary>
        /// Generate Jwt Token
        /// </summary>
        /// <param name="claims">List of Claims</param>
        /// <returns>Jwt Token</returns>
        string GenerateToken(List<Claim> claims);

        /// <summary>
        /// Generate TwoWay Token 
        /// </summary>
        /// <returns>Token</returns>
        string GenerateTwoWayFactoryToken();
        /// <summary>
        /// Generate Refresh Token
        /// </summary>
        /// <returns>Token</returns>
        public string GenerateRefreshToken();
        /// <summary>
        /// Save Token into Header of Request
        /// </summary>
        /// <param name="cookiesDtos">List of cookies to save</param>
        /// <returns><see cref="bool"/> Return true if saved, false if error</returns>
        bool SaveCookies(List<SaveCookiesDtos> cookiesDtos);
    }
}
