using E_BangDomain.Entities;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.TokenRepostitoryDtos;
using E_BangDomain.Settings;
using E_BangDomain.StaticHelper;
using E_BangInfrastructure.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace E_BangInfrastructure.Repository
{
    public sealed class TokenRepository : ITokenRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AuthenticationSettings _authenticationSettings;

        private readonly ProjectDbContext _dbContext;
        public TokenRepository(IHttpContextAccessor httpContextAccessor, AuthenticationSettings authenticationSettings, ProjectDbContext dbContext)
        {
            _httpContextAccessor = httpContextAccessor;
            _authenticationSettings = authenticationSettings;
            _dbContext = dbContext;
        }

        public List<Claim> GenerateClaimsList(Account account, List<string> roles)
        {
            List<Claim> claims =
            [
                new (ClaimTypes.NameIdentifier, account.Id),
                new (ClaimTypes.Name, account.Email!),
                new (ClaimTypes.Email, account.Email!),
                new (ClaimTypes.MobilePhone, account.PhoneNumber!),
            ];

            foreach (var role in roles)
            {
                claims.Add(new(ClaimTypes.Role, role));
            }

            return claims;
        }

        public string GenerateToken(List<Claim> claims)
        {
            SymmetricSecurityKey key = new
                (System.Text.Encoding.UTF8
                .GetBytes(_authenticationSettings.Key));

            SigningCredentials cred =
                new(key, SecurityAlgorithms.HmacSha512Signature);

            JwtSecurityToken token = new(
                                   claims: claims,
                                   expires: DateTime.Now.AddMinutes(_authenticationSettings.ValidMinutes),
                                   issuer: _authenticationSettings.Issure,
                                   audience: _authenticationSettings.Audience,
                                   signingCredentials: cred
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateTwoWayFactoryToken()
            => CodeGenerator.GenerateRandomNumberCode();

        public string GenerateRefreshToken()
            => CodeGenerator.GenerateRandomStringCode();

        public bool SaveCookies(List<SaveCookiesDtos> cookiesDtos)
        {
            HttpContext? context = _httpContextAccessor.HttpContext;
            if (context == null || cookiesDtos == null || cookiesDtos.Count == 0) return false;

            foreach (var cookie in cookiesDtos)
            {
                if (string.IsNullOrEmpty(cookie.Key)
                    || string.IsNullOrEmpty(cookie.Token)
                        || cookie.CookiesOptions == null)
                {
                    continue;
                }

                AppendCookie(context,
                    cookie.Token,
                    cookie.Key,
                    cookie.CookiesOptions);

            }
            return true;
        }
        private void AppendCookie
            (HttpContext httpContext,
            string token,
            string tokenName,
            Action<CookieOptions> options)
        {
            CookieOptions cookieOptions = new();
            options(cookieOptions);
            httpContext.Response.Cookies.Append(tokenName, token, cookieOptions);
        }
        public bool RemoveCookies(List<string> cookies)
        {
            HttpContext? context = _httpContextAccessor.HttpContext;
            if (context == null || cookies == null || cookies.Count == 0)
                return false;
            foreach (var cookie in cookies)
            {
                if (string.IsNullOrEmpty(cookie))
                {
                    continue;
                }
                DeleteCookie(context, cookie);
            }
            return true;
        }

        private void DeleteCookie(HttpContext httpContext, string tokenName)
        {
            httpContext.Response.Cookies.Delete(tokenName);
        }

        public async Task<bool> SaveRefreshTokenAsync(string accountId, string refreshToken, CancellationToken cancellationToken)
        {
            int rowAffected = await _dbContext
                .Account
                .Where(pr => pr.Id == accountId)
                .ExecuteUpdateAsync(pr => pr.SetProperty(p => p.RefreshToken, refreshToken), cancellationToken);

            return rowAffected == 1;
        }
        public async Task<bool> SaveTwoWayFactoryTokenAsync(string accountId, string twoWayToken, CancellationToken token)
        {
            int rowAffected = await _dbContext
                .Account
                .Where(pr => pr.Id == accountId)
                .ExecuteUpdateAsync(pr => pr.SetProperty(p => p.TwoFactoryCode, twoWayToken), token);

            return rowAffected == 1;
        }

        public async Task<string> GetRefreshTokenAsync(string accountId, CancellationToken token)
           => await _dbContext
                .Account
                .Where(pr => pr.Id == accountId)
                .Select(pr => pr.RefreshToken)
                .FirstOrDefaultAsync(token) ?? string.Empty;

    }
}
