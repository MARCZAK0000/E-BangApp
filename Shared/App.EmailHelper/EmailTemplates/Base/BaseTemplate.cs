using App.EmailHelper.Exceptions;
using App.EmailRender.Shared.Abstraction;
using Microsoft.AspNetCore.Components;

namespace App.EmailHelper.EmailTemplates.Base
{
    public abstract class BaseTemplate
    {
        protected async Task<string> RenderComponentTemplateAsync<TComponent, TParameters>(TParameters parameters, IEmailRenderComponent emailRenderComponent)
            where TParameters : IEmailParameters
            where TComponent : IComponent
        {

            string rawTemplate = await emailRenderComponent.RenderComponent<TComponent, TParameters>(parameters);
            if (string.IsNullOrEmpty(rawTemplate))
            {
                throw new EmailTemplateEmptyException("Failed to render the RegistrationAccountComponent.");
            }
            return rawTemplate;
        }
    }
}
