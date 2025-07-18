using E_BangAppEmailBuilder.src.Templates;
using E_BangAppRabbitSharedClass.BuildersDto.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase
{
    public class GenerateTwoWayTokenBody : IGenerateBodyBase
    {
        private readonly IReadTemplates _readTemplates = ReadTemplates.GetInstance();
        public string GenerateBody(object parameters)
        {
            TwoWayTokenBodyBuilder registration = (TwoWayTokenBodyBuilder)parameters;
            string template = _readTemplates.GetDefaultBodyTemplate(registration.TemplateName);
            if (string.IsNullOrEmpty(template))
            {
                throw new InvalidOperationException("Empty Body Template");
            }
            return template.Replace("[email]", registration.Email).Replace("[token]", registration.Token);
        }
    }
}
