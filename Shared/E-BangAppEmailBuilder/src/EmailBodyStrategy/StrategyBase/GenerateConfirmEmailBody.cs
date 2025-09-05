using E_BangAppEmailBuilder.src.Templates;
using E_BangAppRabbitSharedClass.BuildersDto.Body;
using E_BangAppRabbitSharedClass.BuildersDto.Body.BodyBuilder;
using E_BangAppRabbitSharedClass.Exceptions;
using System.Text.Json;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase
{
    internal class GenerateConfirmEmailBody : IGenerateBodyBase
    {
        private readonly IReadTemplates _readTemplates = ReadTemplates.GetInstance();
        public string GenerateBody(JsonElement parameters)
        {
            var confirmEmail = JsonSerializer.Deserialize<ConfirmEmailTokenBodyBuilder>(parameters)
               ?? throw new InvalidDataSerializationException($"Invalid serialization type: {nameof(ConfirmEmailTokenBodyBuilder)}");
            string template = _readTemplates.GetDefaultBodyTemplate(confirmEmail.TemplateName??string.Empty);
            if (string.IsNullOrEmpty(template))
            {
                throw new EmailTemplatedNotFoundException($"Empty Body Template: {confirmEmail.TemplateName ?? nameof(ConfirmEmailTokenBodyBuilder)}");
            }

            return template.Replace("[verification-token]", confirmEmail.Token??string.Empty);
            
        }
    }
}
    