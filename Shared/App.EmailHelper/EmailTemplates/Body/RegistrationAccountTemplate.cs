using App.EmailRender.Shared.Abstraction;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace App.EmailHelper.EmailTemplates.Body
{
    public class RegistrationAccountTemplate : IEmailTemplate
    {
        private readonly IEmailRenderComponent _emailRenderComponent;

        private readonly ILogger<RegistrationAccountTemplate> _logger;  

        public RegistrationAccountTemplate(IEmailRenderComponent emailRenderComponent, ILogger<RegistrationAccountTemplate> logger)
        {
            _logger = logger;
            _emailRenderComponent = emailRenderComponent;
        }
        public Task<string> RenderTemplateAsync<TParameters>(TParameters parameters) where TParameters : IEmailParameters
        {
            throw new NotImplementedException();
        }
    }
}
