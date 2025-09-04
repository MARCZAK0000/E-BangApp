using E_BangAppEmailBuilder.src.Templates;
using E_BangAppRabbitSharedClass.BuildersDto.Body;
using E_BangAppRabbitSharedClass.BuildersDto.Body.BodyBuilder;
using System.Text.Json;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase
{
    internal class GenerateConfirmEmailBody : IGenerateBodyBase
    {
        private readonly IReadTemplates _readTemplates = ReadTemplates.GetInstance();
        public string GenerateBody(JsonElement parameters)
        {
            var confirmEmail = JsonSerializer.Deserialize<RegistrationBodyBuilder>(parameters)
               ?? throw new InvalidOperationException("Invalid parameters type");
            string template = _readTemplates.GetDefaultBodyTemplate(confirmEmail.TemplateName);
            if (string.IsNullOrEmpty(template))
            {
                throw new InvalidOperationException("Empty Body Template");
            }

            return template.Replace("[verification-token]", confirmEmail.Token??string.Empty);
            
        }
    }
}
