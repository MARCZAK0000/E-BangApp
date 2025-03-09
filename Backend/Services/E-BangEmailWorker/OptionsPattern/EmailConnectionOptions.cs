namespace E_BangEmailWorker.OptionsPattern
{
    public class EmailConnectionOptions
    {

        public required string EmailName { get; set; }
        public required string Password { get; set; }
        public required string SmptHost { get; set; }
        public int Port { get; set; }
        public required string Salt { get; set; }

        public override bool Equals(object? obj)
        {

            if (obj is not EmailSettings values) return false;
            return values.EmailName == EmailName
                && values.Port == Port
                    && values.SmptHost == SmptHost
                        && values.Salt == Salt;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(EmailName, Password, SmptHost, Port);
        }
    }
}