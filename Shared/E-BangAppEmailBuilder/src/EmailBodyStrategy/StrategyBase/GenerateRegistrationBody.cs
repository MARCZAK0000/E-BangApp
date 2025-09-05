using E_BangAppEmailBuilder.src.Templates;
using E_BangAppRabbitSharedClass.BuildersDto.Body.BodyBuilder;
using E_BangAppRabbitSharedClass.Exceptions;
using System.Text.Json;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase
{
    internal class GenerateRegistrationBody : IGenerateBodyBase
    {
        private readonly IReadTemplates _readTemplates = ReadTemplates.GetInstance();
        public string GenerateBody(JsonElement parameters)
        {
            var registrationBody = JsonSerializer.Deserialize<RegistrationBodyBuilder>(parameters)
                ?? throw new InvalidDataSerializationException($"Invalid serialization type: {nameof(RegistrationBodyBuilder)}");
            string template = _readTemplates.GetDefaultBodyTemplate(registrationBody.TemplateName ?? string.Empty);
            if (string.IsNullOrEmpty(template))
            {
                throw new EmailTemplatedNotFoundException($"Empty Body Template: {registrationBody.TemplateName ?? nameof(RegistrationBodyBuilder)}");
            }
            return template
                .Replace("[email]", registrationBody.Email ?? string.Empty)
                .Replace("[token]", registrationBody.Token ?? string.Empty);
        }
    }
}
