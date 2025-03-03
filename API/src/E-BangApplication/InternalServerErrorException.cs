namespace E_BangApplication
{
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException(string? message) : base(message)
        {
        }
    }
}
