namespace E_BangApplication.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException(string? message) : base(message)
        {
        }
    }
}
