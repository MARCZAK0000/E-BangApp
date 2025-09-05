using E_BangAppEmailBuilder.src.Templates;
using E_BangAppRabbitSharedClass.BuildersDto.Body.BodyBuilder;
using E_BangAppRabbitSharedClass.Exceptions;
using System.Text.Json;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase
{
    public class GenerateChangePasswordBody : IGenerateBodyBase
    {
        private readonly IReadTemplates _readTemplates = ReadTemplates.GetInstance();
        public string GenerateBody(JsonElement parameters)
        {
            ChangePasswordBodyBuilder changePasswordBody = JsonSerializer.Deserialize<ChangePasswordBodyBuilder>(parameters)
                ?? throw new InvalidDataSerializationException($"Invalid serialization type: {nameof(ChangePasswordBodyBuilder)}");

            string template = _readTemplates.GetDefaultBodyTemplate(changePasswordBody.TemplateName??string.Empty);
            if (string.IsNullOrEmpty(template))
            {
                throw new EmailTemplatedNotFoundException($"Empty Body Template: {changePasswordBody.TemplateName ?? nameof(ChangePasswordBodyBuilder)}");
            }
            return template
                .Replace("[email]", changePasswordBody.Email ?? string.Empty)
                .Replace("[token]", changePasswordBody.Token ?? string.Empty)
                .Replace("[link]", changePasswordBody.PageUrl ?? string.Empty);
        }
    }
}
