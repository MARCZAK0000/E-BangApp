using App.EmailHelper.EmailComponents.Body;
using App.EmailHelper.EmailParameters.Body;
using App.EmailHelper.EmailTemplates.Base;
using App.EmailHelper.Exceptions;
using App.EmailRender.Shared.Abstraction;
using Microsoft.Extensions.Logging;

namespace App.EmailHelper.EmailTemplates.Body
{
    public class TwoWayTokenTemplate : BaseTemplate, IEmailTemplate
    {
        private readonly IEmailRenderComponent _emailRenderComponent;
        private readonly ILogger<TwoWayTokenTemplate> _logger;
        public TwoWayTokenTemplate(IEmailRenderComponent emailRenderComponent, ILogger<TwoWayTokenTemplate> logger)
        {
            _logger = logger;
            _emailRenderComponent = emailRenderComponent;
        }
        public async Task<string> RenderTemplateAsync<TParameters>(TParameters parameters) where TParameters : IEmailParameters
        {
            try
            {
                if (parameters is not TwoWayTokenParameters typedParameters)
                {
                    throw new EmailParametersEmptyException("Invalid parameters type. Expected TwoWayTokenParameters.");
                }
                string result = await RenderComponentTemplateAsync<TwoWayTokenComponent, TwoWayTokenParameters>
                    (typedParameters, _emailRenderComponent);
                return result;
            }
            catch (EmailParametersEmptyException e)
            {
                _logger.LogError(e, "{Date} - {ClassName} : {ErrorMessage}", DateTime.UtcNow, nameof(TwoWayTokenTemplate), e.Message);
                throw;
            }
            catch (EmailTemplateEmptyException e)
            {
                _logger.LogError(e, "{Date} - {ClassName} : {ErrorMessage}", DateTime.UtcNow, nameof(TwoWayTokenTemplate), e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError("{Date} - {ClassName} : {ErrorMessage}", DateTime.UtcNow, nameof(TwoWayTokenTemplate),
                    "An unexpected error occurred while rendering the email template." + e.Message);
                throw;
            }
        }
    }
}
