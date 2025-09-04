using E_BangAppEmailBuilder.src.Templates;
using E_BangAppRabbitSharedClass.BuildersDto.Body;
using E_BangAppRabbitSharedClass.BuildersDto.Body.BodyBuilder;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase
{
    internal class GenerateRegistrationBody : IGenerateBodyBase
    {
        private readonly IReadTemplates _readTemplates = ReadTemplates.GetInstance();
        public string GenerateBody<T>(T parameters)
        {
            if(parameters is RegistrationBodyBuilder registrationBody)
            {
                string template = _readTemplates.GetDefaultBodyTemplate(registrationBody.TemplateName);
                if (string.IsNullOrEmpty(template))
                {
                    throw new InvalidOperationException("Empty Body Template");
                }
                
                return template
                    .Replace("[email]", registrationBody.Email ?? string.Empty)
                    .Replace("[token]", registrationBody.Token ?? string.Empty);
            }
            throw new InvalidOperationException("Invalid parameters type");
        }
    }
}
