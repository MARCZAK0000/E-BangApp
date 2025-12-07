namespace E_BangEmailWorker.Exceptions
{
    public class EmailConfigurationIsNotEqualSoruceException : Exception
    {
        public EmailConfigurationIsNotEqualSoruceException()
        {
        }

        public EmailConfigurationIsNotEqualSoruceException(string? message) : base(message)
        {
        }

        public EmailConfigurationIsNotEqualSoruceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
