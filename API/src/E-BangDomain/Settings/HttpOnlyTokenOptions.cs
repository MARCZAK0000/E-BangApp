namespace E_BangDomain.Settings
{
    public class HttpOnlyTokenOptions
    {
        public bool IsHttpOnly { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTimeOffset AccessTokenExpireDate { get; set; }
        public DateTimeOffset RefreshTokenExpireDate { get; set; }
    }
}
