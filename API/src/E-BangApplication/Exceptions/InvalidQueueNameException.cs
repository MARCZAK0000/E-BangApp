namespace E_BangApplication.Exceptions
{
    public class InvalidQueueNameException : Exception
    {
        public InvalidQueueNameException()
        {
        }

        public InvalidQueueNameException(string? message) : base(message)
        {
        }
    }
}
