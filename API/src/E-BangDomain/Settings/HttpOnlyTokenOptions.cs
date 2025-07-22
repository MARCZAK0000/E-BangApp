namespace E_BangDomain.Settings
{
    public class HttpOnlyTokenOptions
    {
        public bool IsHttpOnly { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int AccessTokenExpireDate { get; set; }
        public int RefreshTokenExpireDate { get; set; }
    }
}
