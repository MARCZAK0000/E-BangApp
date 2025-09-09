using App.EmailHelper.EmailParameters.Body;
using App.EmailHelper.EmailTemplates.Base;
using App.EmailHelper.Exceptions;
using App.EmailRender.Shared.Abstraction;
using EmailComponents.Body;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.EmailHelper.EmailTemplates.Body
{
    public class ConfirmEmailTemplate : BaseTemplate, IEmailTemplate
    {
        private readonly IEmailRenderComponent _emailRenderComponent;

        private readonly ILogger<ConfirmEmailTemplate> _logger;

        public ConfirmEmailTemplate(IEmailRenderComponent emailRenderComponent, ILogger<ConfirmEmailTemplate> logger)
        {
            _emailRenderComponent = emailRenderComponent;
            _logger = logger;
        }

        public async Task<string> RenderTemplateAsync<TParameters>(TParameters parameters) where TParameters : IEmailParameters
        {
            try
            {
                if(parameters is not ConfimEmailParameters confimEmailParameters)
                {
                    throw new EmailParametersEmptyException("Invalid parameters type. Expected ConfimEmailParameters.");
                }
                string result = await RenderComponentTemplateAsync<ConfirmEmailComponent, ConfimEmailParameters>
                    (confimEmailParameters, _emailRenderComponent);
                return result;
            }
            catch (EmailTemplateEmptyException ex)
            {
                _logger.LogError(ex, "{Date} - {ClassName} : {ErrorMessage}", DateTime.UtcNow, nameof(ConfirmEmailTemplate), ex.Message);
                throw;
            }
            catch (EmailParametersEmptyException ex)
            {
                _logger.LogError(ex, "{Date} - {ClassName} : {ErrorMessage}", DateTime.UtcNow, nameof(ConfirmEmailTemplate), ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("{Date} - {ClassName} : {ErrorMessage}", DateTime.UtcNow, nameof(ConfirmEmailTemplate),
                    "An unexpected error occurred while rendering the email template."+ex.Message);
                throw;
            }
        }
    }
}
