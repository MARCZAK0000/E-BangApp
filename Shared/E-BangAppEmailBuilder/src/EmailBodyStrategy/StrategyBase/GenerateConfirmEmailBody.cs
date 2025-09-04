using E_BangAppEmailBuilder.src.Templates;
using E_BangAppRabbitSharedClass.BuildersDto.Body;
using E_BangAppRabbitSharedClass.BuildersDto.Body.BodyBuilder;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase
{
    internal class GenerateConfirmEmailBody : IGenerateBodyBase
    {
        private readonly IReadTemplates _readTemplates = ReadTemplates.GetInstance();
        public string GenerateBody<T>(T parameters)
        {
            if(parameters is ConfirmEmailTokenBodyBuilder bodyBuilder)
            {
                string template = _readTemplates.GetDefaultBodyTemplate(bodyBuilder.TemplateName);
                if (string.IsNullOrEmpty(template))
                {
                    throw new InvalidOperationException("Empty Body Template");
                }
                return template.Replace("[verification-token]", bodyBuilder.Token??string.Empty);
            }
            throw new InvalidOperationException("Invalid parameters type");
        }
    }
}
