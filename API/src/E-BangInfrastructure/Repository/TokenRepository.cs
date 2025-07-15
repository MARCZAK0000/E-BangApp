using E_BangDomain.Entities;
using E_BangDomain.Repository;
using E_BangDomain.RequestDtos.TokenRepostitoryDtos;
using E_BangDomain.Settings;
using E_BangDomain.StaticHelper;
using Microsoft.AspNetCore.Http;

namespace E_BangInfrastructure.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly HttpOnlyTokenOptions _httpOnlyTokenOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenRepository(HttpOnlyTokenOptions httpOnlyTokenOptions, IHttpContextAccessor httpContextAccessor)
        {
            _httpOnlyTokenOptions = httpOnlyTokenOptions;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateJWTTokenAsync()
        {
            throw new NotImplementedException();
        }

        public string GenerateTwoWayFactoryToken() 
            => CodeGenerator.GenerateRandomNumberCode();

        public string GenerateRefreshTokenAsync()
            => CodeGenerator.GenerateRandomStringCode();

        public bool SaveCookiesAsync(SaveCookiesDtos cookiesDtos)
        {
            HttpContext? context = _httpContextAccessor.HttpContext;
            if(context == null)
            {
                return false;
            }
            context.Response.Cookies.Append(_httpOnlyTokenOptions.AccessToken, cookiesDtos.AccessToken, new CookieOptions
            {
                Expires = _httpOnlyTokenOptions.AccessTokenExpireDate,
                HttpOnly = _httpOnlyTokenOptions.IsHttpOnly,
                IsEssential = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
            });

            context.Response.Cookies.Append(_httpOnlyTokenOptions.RefreshToken, cookiesDtos.RefreshToken, new CookieOptions
            {
                Expires = _httpOnlyTokenOptions.RefreshTokenExpireDate,
                HttpOnly = _httpOnlyTokenOptions.IsHttpOnly,
                IsEssential = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
            });

            return true;
        }
    }
}
