using E_BangApplication.Exceptions;
using E_BangDomain.Settings;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace E_BangApplication.Authentication
{
    public class UserContext : IUserContext
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;

        private readonly HttpOnlyTokenOptions _httpOnlyTokenOptions;

        public UserContext(IHttpContextAccessor httpContextAccessor
            ,HttpOnlyTokenOptions httpOnlyTokenOptions)
        {
            _HttpContextAccessor = httpContextAccessor;
            _httpOnlyTokenOptions = httpOnlyTokenOptions;
        }

        public CurrentUser GetCurrentUser()
        {
            var user = _HttpContextAccessor.HttpContext!.User;
            if (user == null || !user.Identity!.IsAuthenticated)
            {
                throw new UnAuthorizedExceptions("UnAuthorized User");
            }
            string accountId = user.FindFirst(pr => pr.Type == ClaimTypes.NameIdentifier)!.Value;
            string email = user.FindFirst(pr => pr.Type == ClaimTypes.Email)!.Value;
            List<string> roles = user.FindAll(pr => pr.Type == ClaimTypes.Role).Select(s => s.Value).ToList();
            return new CurrentUser(accountId, email, roles);
        }

        public string GetRefreshToken()
        {
            return _HttpContextAccessor.HttpContext!.Request.Cookies[_httpOnlyTokenOptions.RefreshToken] ?? 
                throw new UnAuthorizedExceptions("Refresh token not found");
        }
    }
}
