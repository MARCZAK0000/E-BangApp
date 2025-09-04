using E_BangAppEmailBuilder.src.Templates;
using E_BangAppRabbitSharedClass.BuildersDto.Body;
using E_BangAppRabbitSharedClass.BuildersDto.Body.BodyBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_BangAppEmailBuilder.src.EmailBodyStrategy.StrategyBase
{
    public class GenerateTwoWayTokenBody : IGenerateBodyBase
    {
        private readonly IReadTemplates _readTemplates = ReadTemplates.GetInstance();
        
        public string GenerateBody(JsonElement parameters)
        {
            var twoWayToken = JsonSerializer.Deserialize<RegistrationBodyBuilder>(parameters)
              ?? throw new InvalidOperationException("Invalid parameters type");
            
            string template = _readTemplates.GetDefaultBodyTemplate(twoWayToken.TemplateName);
            
            if (string.IsNullOrEmpty(template))
            {
                throw new InvalidOperationException("Empty Body Template");
            }
            
            return template
                .Replace("[email]", twoWayToken.Email ?? string.Empty)
                .Replace("[token]", twoWayToken.Token ?? string.Empty);
        }
    }
}
