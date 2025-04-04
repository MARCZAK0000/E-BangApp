namespace E_BangApplication.Exceptions
{
    public class EmptyRabbitMessageException : Exception
    {
        public EmptyRabbitMessageException()
        {
        }

        public EmptyRabbitMessageException(string? message) : base(message)
        {
        }
    }
}
