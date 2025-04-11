using E_BangAppEmailBuilder.src.Builder;

namespace E_BangAppEmailBuilder.src.Abstraction
{
    public class BuilderEmail : IBuilderEmail
    {
        public EmailMessage GenerateMessage(object header, object body, object footer)
        {
            var emailBuilder = new EmailBuilder();

            return emailBuilder
                .GenerateHeader(header)
                .GenerateBody(body)
                .GenerateFooter(footer)
                .BuildMessage();
        }
    }
}
