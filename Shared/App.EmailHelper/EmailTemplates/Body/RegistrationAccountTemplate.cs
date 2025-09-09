using App.EmailHelper.EmailComponents.Body;
using App.EmailHelper.EmailParameters.Body;
using App.EmailHelper.EmailTemplates.Base;
using App.EmailHelper.Exceptions;
using App.EmailRender.Shared.Abstraction;
using Microsoft.Extensions.Logging;

namespace App.EmailHelper.EmailTemplates.Body
{
    public class RegistrationAccountTemplate : BaseTemplate, IEmailTemplate
    {
        private readonly IEmailRenderComponent _emailRenderComponent;

        private readonly ILogger<RegistrationAccountTemplate> _logger;

        public RegistrationAccountTemplate(IEmailRenderComponent emailRenderComponent, ILogger<RegistrationAccountTemplate> logger)
        {
            _logger = logger;
            _emailRenderComponent = emailRenderComponent;
        }
        public async Task<string> RenderTemplateAsync<TParameters>(TParameters parameters) where TParameters : IEmailParameters
        {
            try
            {
                if (parameters is not RegistrationAccountParameters typedParameters)
                {
                    throw new EmailParametersEmptyException("Invalid parameters type. Expected RegistrationAccountParameters.");
                }
                var result = await RenderComponentTemplateAsync<RegistrationAccountComponent, RegistrationAccountParameters>
                    (typedParameters, _emailRenderComponent);
                return result;
            }
            catch (EmailParametersEmptyException ex)
            {
                _logger.LogError(ex, "{Date} - {ClassName} : {ErrorMessage}", DateTime.UtcNow, nameof(RegistrationAccountTemplate), ex.Message);
                throw;
            }
            catch (EmailTemplateEmptyException err)
            {
                _logger.LogError(err, "{Date} - {ClassName} : {ErrorMessage}", DateTime.UtcNow, nameof(RegistrationAccountTemplate), err.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Date} - {ClassName} : {ErrorMessage}", DateTime.UtcNow, nameof(RegistrationAccountTemplate), e.Message);
                throw;
            }


        }
    }
}
