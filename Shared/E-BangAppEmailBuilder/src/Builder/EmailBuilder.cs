using E_BangAppEmailBuilder.src.BuildersDto.Header;
using E_BangAppEmailBuilder.src.Enums;

namespace E_BangAppEmailBuilder.src.Builder
{
    public class EmailBuilder
    {
        private EmailMessage response = new EmailMessage();
        /// <summary>
        /// Generate Default Header Template
        /// </summary>
        /// <param name="options">Insert Email Address</param>
        /// <returns>Chain Method</returns>
        public EmailBuilder GenerateHeader(HeaderDefaultBuilderOptions options)
        {
            response.Header = options.Email;
            return this;
        }
        public EmailBuilder GenerateHeader(HeaderCustomBuilderOptions options)
        {
            response.Header = options.CustomMessage; 
            return this;
        }
        public EmailBuilder GenerateBody(string email)
        {

        }
    }
}
