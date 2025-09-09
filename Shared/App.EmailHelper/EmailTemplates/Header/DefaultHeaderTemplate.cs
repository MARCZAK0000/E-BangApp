using App.EmailHelper.EmailComponents.Header;
using App.EmailHelper.EmailParameters.Header;
using App.EmailHelper.EmailTemplates.Base;
using App.EmailHelper.Exceptions;
using App.EmailRender.Shared.Abstraction;
using Microsoft.Extensions.Logging;
namespace App.EmailHelper.EmailTemplates.Header
{
    public class DefaultHeaderTemplate : BaseTemplate, IEmailTemplate
    {
        private readonly IEmailRenderComponent _emailRenderComponent;

        private readonly ILogger<DefaultHeaderTemplate> _logger;

        public DefaultHeaderTemplate(IEmailRenderComponent emailRenderComponent,
            ILogger<DefaultHeaderTemplate> logger)
        {
            _emailRenderComponent = emailRenderComponent;
            _logger = logger;
        }

        public async Task<string> RenderTemplateAsync<TParameters>(TParameters parameters) where TParameters : IEmailParameters
        {
            try
            {
                if (parameters is not DefaultHeaderParameters confimEmailParameters)
                {
                    throw new EmailParametersEmptyException("Invalid parameters type. Expected ConfimEmailParameters.");
                }
                string result = await RenderComponentTemplateAsync<DefaultHeaderComponent, DefaultHeaderParameters>
                    (confimEmailParameters, _emailRenderComponent);
                return result;
            }
            catch (EmailTemplateEmptyException ex)
            {
                _logger.LogError(ex, "{Date} - {ClassName} : {ErrorMessage}", DateTime.UtcNow, nameof(DefaultHeaderTemplate), ex.Message);
                throw;
            }
            catch (EmailParametersEmptyException ex)
            {
                _logger.LogError(ex, "{Date} - {ClassName} : {ErrorMessage}", DateTime.UtcNow, nameof(DefaultHeaderTemplate), ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("{Date} - {ClassName} : {ErrorMessage}", DateTime.UtcNow, nameof(DefaultHeaderTemplate),
                    "An unexpected error occurred while rendering the email template." + ex.Message);
                throw;
            }
        }
    }
}
