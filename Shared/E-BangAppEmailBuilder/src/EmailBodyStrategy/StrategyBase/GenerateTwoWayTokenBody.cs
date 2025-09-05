using E_BangAppEmailBuilder.src.Templates;
using E_BangAppRabbitSharedClass.BuildersDto.Body.BodyBuilder;
using E_BangAppRabbitSharedClass.Exceptions;
using System.Text.Json;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase
{
    public class GenerateTwoWayTokenBody : IGenerateBodyBase
    {
        private readonly IReadTemplates _readTemplates = ReadTemplates.GetInstance();

        public string GenerateBody(JsonElement parameters)
        {
            var twoWayToken = JsonSerializer.Deserialize<TwoWayTokenBodyBuilder>(parameters)
              ?? throw new InvalidDataSerializationException($"Invalid serialization type: {nameof(TwoWayTokenBodyBuilder)}");

            string template = _readTemplates.GetDefaultBodyTemplate(twoWayToken.TemplateName ?? string.Empty);

            if (string.IsNullOrEmpty(template))
            {
                throw new EmailTemplatedNotFoundException($"Empty Body Template: {twoWayToken.TemplateName ?? nameof(TwoWayTokenBodyBuilder)}");
            }

            return template
                .Replace("[email]", twoWayToken.Email ?? string.Empty)
                .Replace("[token]", twoWayToken.Token ?? string.Empty);
        }
    }
}
