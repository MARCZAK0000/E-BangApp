namespace E_BangEmailWorker.Exceptions
{
    public class MesseageNotSendException : Exception
    {
        public MesseageNotSendException()
        {
        }

        public MesseageNotSendException(string? message) : base(message)
        {
        }
    }
}
