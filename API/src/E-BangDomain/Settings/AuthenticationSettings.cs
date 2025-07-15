namespace E_BangDomain.Settings
{
    public class AuthenticationSettings
    {
        public string Issuer { get; set; }
        public string Key { get; set; }
        public string Audience { get; set; }
        public int ValidMinutes { get; set; }
    }
}
