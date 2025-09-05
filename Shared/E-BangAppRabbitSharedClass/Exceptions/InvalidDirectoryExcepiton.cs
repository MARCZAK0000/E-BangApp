namespace E_BangAppRabbitSharedClass.Exceptions
{
    public class InvalidDirectoryExcepiton : Exception
    {
        public InvalidDirectoryExcepiton()
        {
        }

        public InvalidDirectoryExcepiton(string? message) : base(message)
        {
        }
    }
}
