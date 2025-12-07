namespace E_BangEmailWorker.Exceptions
{
    public class EmailConfigurationNullException : Exception
    {
        public EmailConfigurationNullException()
        {
        }

        public EmailConfigurationNullException(string? message) : base(message)
        {
        }

        public EmailConfigurationNullException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
