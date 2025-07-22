namespace E_BangApplication.Exceptions
{
    public class UnAuthorizedExceptions : Exception
    {
        public UnAuthorizedExceptions()
        {
        }

        public UnAuthorizedExceptions(string? message) : base(message)
        {
        }
    }
}
