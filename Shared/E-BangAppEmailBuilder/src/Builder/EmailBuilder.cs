namespace E_BangAppEmailBuilder.src.Builder
{
    public class EmailBuilder
    {
        private EmailMessage response = new EmailMessage();

        public EmailBuilder GenerateHeader(string email, string? customeMessage = null)
        {
            if (customeMessage != null)
            {
                response.Header = customeMessage;
                return this;
            }

            return this;
        }

        public EmailBuilder GenerateBody(string email)
        {

        }
    }
}
