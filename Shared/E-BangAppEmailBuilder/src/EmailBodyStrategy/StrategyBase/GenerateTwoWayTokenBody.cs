using E_BangAppEmailBuilder.src.Templates;
using E_BangAppRabbitSharedClass.BuildersDto.Body;
using E_BangAppRabbitSharedClass.BuildersDto.Body.BodyBuilder;
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
        public string GenerateBody<T>(T parameters)
        {
            if(parameters is TwoWayTokenBodyBuilder builder)
            {
                string template = _readTemplates.GetDefaultBodyTemplate(builder.TemplateName);
                if (string.IsNullOrEmpty(template))
                {
                    throw new InvalidOperationException("Empty Body Template");
                }
                return template.Replace("[email]", builder.Email).Replace("[token]", builder.Token);
            }
            throw new InvalidOperationException("Invalid Parameters");

        }
    }
}
