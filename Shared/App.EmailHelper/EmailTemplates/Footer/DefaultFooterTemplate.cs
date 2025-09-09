using App.EmailHelper.EmailComponents.Footer;
using App.EmailHelper.EmailParameters.Footer;
using App.EmailHelper.EmailTemplates.Base;
using App.EmailHelper.Exceptions;
using App.EmailRender.Shared.Abstraction;
using Microsoft.Extensions.Logging;
namespace App.EmailHelper.EmailTemplates.Footer
{
    public class DefaultFooterTemplate : BaseTemplate, IEmailTemplate
    {
        private readonly IEmailRenderComponent _emailRenderComponent;

        private readonly ILogger<DefaultFooterTemplate> _logger;

        public DefaultFooterTemplate(IEmailRenderComponent emailRenderComponent,
            ILogger<DefaultFooterTemplate> logger)
        {
            _emailRenderComponent = emailRenderComponent;
            _logger = logger;
        }

        public async Task<string> RenderTemplateAsync<TParameters>(TParameters parameters) where TParameters : IEmailParameters
        {
            try
            {
                if (parameters is not DefaultFooterParameters confimEmailParameters)
                {
                    throw new EmailParametersEmptyException("Invalid parameters type. Expected ConfimEmailParameters.");
                }
                string result = await RenderComponentTemplateAsync<DefaultFooterComponent, DefaultFooterParameters>
                    (confimEmailParameters, _emailRenderComponent);
                return result;
            }
            catch (EmailTemplateEmptyException ex)
            {
                _logger.LogError(ex, "{Date} - {ClassName} : {ErrorMessage}", DateTime.UtcNow, nameof(DefaultFooterTemplate), ex.Message);
                throw;
            }
            catch (EmailParametersEmptyException ex)
            {
                _logger.LogError(ex, "{Date} - {ClassName} : {ErrorMessage}", DateTime.UtcNow, nameof(DefaultFooterTemplate), ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("{Date} - {ClassName} : {ErrorMessage}", DateTime.UtcNow, nameof(DefaultFooterTemplate),
                    "An unexpected error occurred while rendering the email template." + ex.Message);
                throw;
            }
        }
    }
}
