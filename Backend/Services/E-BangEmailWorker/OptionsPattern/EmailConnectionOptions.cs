using E_BangEmailWorker.Database;

namespace E_BangEmailWorker.OptionsPattern
{
    public class EmailConnectionOptions
    {

        public string EmailName { get; set; }
        public string Password { get; set; }
        public string SmptHost { get; set; }
        public int Port { get; set; }
        public string Salt { get; set; }

        public override bool Equals(object? obj)
        {

            if (obj is not EmailSetting values) return false;
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