namespace E_BangApplication.Exceptions
{
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException(string? message) : base(message)
        {
        }
    }
}
