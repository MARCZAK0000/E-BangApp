namespace E_BangAppEmailBuilder.src.Builder
{
    public class EmailMessage
    {
        public string Message { get; private set; }

        //private readonly byte[]? Attachments { get; set; }
        public EmailMessage(string message)
        {
            Message = message;
        }
        
    }
}
