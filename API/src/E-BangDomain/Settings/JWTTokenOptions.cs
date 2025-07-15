namespace E_BangDomain.Settings
{
    public class JWTTokenOptions
    {
        public string Key { get; set; }
        public string Issure { get; set; }
        public int ExpireMinutes { get; set; }
        public string Audience { get; set; }
    }
}
