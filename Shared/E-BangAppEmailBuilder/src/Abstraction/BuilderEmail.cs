using E_BangAppEmailBuilder.src.Builder;
using E_BangAppRabbitSharedClass.BuildersDto.Footer;
using E_BangAppRabbitSharedClass.BuildersDto.Header;
using E_BangAppRabbitSharedClass.Enums;
using System.Text.Json;

namespace E_BangAppEmailBuilder.src.Abstraction
{
    public class BuilderEmail : IBuilderEmail
    {
        public EmailMessage GenerateMessage(HeaderDefaultTemplateBuilder header, 
            (EEnumEmailBodyBuilderType type, JsonElement element) body, FooterDefualtTemplateBuilder footer)
        {
            var emailBuilder = new EmailBuilder();

            return emailBuilder
                .GenerateHeader(header)
                .GenerateBody(body.type, body.element)
                .GenerateFooter(footer)
                .BuildMessage();
        }
    }
}
