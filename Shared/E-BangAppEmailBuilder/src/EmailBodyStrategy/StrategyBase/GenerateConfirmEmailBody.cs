using E_BangAppEmailBuilder.src.Templates;
using E_BangAppRabbitSharedClass.BuildersDto.Body;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase
{
    internal class GenerateConfirmEmailBody : IGenerateBodyBase
    {
        private readonly IReadTemplates _readTemplates = ReadTemplates.GetInstance();
        public string GenerateBody(object parameters)
        {
            ConfirmEmailTokenBodyBuilder registration = (ConfirmEmailTokenBodyBuilder)parameters;
            string template = _readTemplates.GetDefaultBodyTemplate(registration.TemplateName);
            if (string.IsNullOrEmpty(template))
            {
                throw new InvalidOperationException("Empty Body Template");
            }
            return template.Replace("[verification-token]", registration.Token);
        }
    }
}
