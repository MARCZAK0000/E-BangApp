using Microsoft.AspNetCore.Http;

namespace E_BangDomain.RequestDtos.TokenRepostitoryDtos
{
    public class SaveCookiesDtos
    {
        public string Token { get; set; }
        public string Key { get; set; }
        public Action<CookieOptions> CookiesOptions { get; set; }
    }
}
