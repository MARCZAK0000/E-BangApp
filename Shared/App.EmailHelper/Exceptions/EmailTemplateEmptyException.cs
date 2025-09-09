namespace App.EmailHelper.Exceptions
{
    public class EmailTemplateEmptyException : Exception
    {
        public EmailTemplateEmptyException()
        {
        }

        public EmailTemplateEmptyException(string? message) : base(message)
        {
        }
    }
}
