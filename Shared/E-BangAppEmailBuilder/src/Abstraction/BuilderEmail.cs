using E_BangAppEmailBuilder.src.Builder;
using E_BangAppRabbitSharedClass.BuildersDto.Footer;
using E_BangAppRabbitSharedClass.BuildersDto.Header;

namespace E_BangAppEmailBuilder.src.Abstraction
{
    public class BuilderEmail : IBuilderEmail
    {
        public EmailMessage GenerateMessage(HeaderDefaultTemplateBuilder header, object body, FooterDefualtTemplateBuilder footer)
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
