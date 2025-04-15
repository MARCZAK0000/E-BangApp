using E_BangAppEmailBuilder.src.Templates;
using E_BangAppRabbitSharedClass.BuildersDto.Body;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase
{
    internal class GenerateRegistrationBody : IGenerateBodyBase
    {
        private readonly IReadTemplates _readTemplates = ReadTemplates.GetInstance();
        public string GenerateBody(object parameters)
        {
            RegistrationBodyBuilder registration = (RegistrationBodyBuilder)parameters;
            string template = _readTemplates.GetDefaultBodyTemplate(registration.TemplateName);
            if (string.IsNullOrEmpty(template))
            {
                throw new InvalidOperationException("Empty Body Template");
            }
            return template.Replace("[email]", registration.Email).Replace("[token]", registration.Token);
        }
    }
}
