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

        /// <summary>
        /// Saves a two-way factory token for the specified account asynchronously.
        /// </summary>
        /// <remarks>This method performs the save operation asynchronously and may involve external
        /// storage or services. Ensure that the provided <paramref name="accountId"/> and <paramref
        /// name="twoWayToken"/> are valid.</remarks>
        /// <param name="accountId">The unique identifier of the account for which the token is being saved. Must not be <see langword="null"/>
        /// or empty.</param>
        /// <param name="twoWayToken">The two-way factory token to save. Must not be <see langword="null"/> or empty.</param>
        /// <param name="token">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns><see langword="true"/> if the token was successfully saved; otherwise, <see langword="false"/>.</returns>
        Task<bool> SaveTwoWayFactoryTokenAsync(string accountId, string twoWayToken, CancellationToken token);

        /// <summary>
        /// Saves the specified refresh token for the given account asynchronously.
        /// </summary>
        /// <remarks>This method performs the save operation asynchronously and may involve I/O
        /// operations. Ensure that the provided <paramref name="cancellationToken"/> is used to handle cancellation
        /// scenarios.</remarks>
        /// <param name="accountId">The unique identifier of the account for which the refresh token is being saved. Must not be <see
        /// langword="null"/> or empty.</param>
        /// <param name="refreshToken">The refresh token to save. Must not be <see langword="null"/> or empty.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns><see langword="true"/> if the refresh token was successfully saved; otherwise, <see langword="false"/>.</returns>
        Task<bool> SaveRefreshTokenAsync(string accountId, string refreshToken, CancellationToken cancellationToken);

    }
}
