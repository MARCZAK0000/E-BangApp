namespace App.EmailHelper.Exceptions
{
    public class EmailParametersEmptyException : Exception
    {
        public EmailParametersEmptyException()
        {
        }

        public EmailParametersEmptyException(string? message) : base(message)
        {
        }
    }
}
