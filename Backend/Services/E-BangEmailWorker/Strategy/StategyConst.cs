using App.EmailHelper.EmailParameters.Body;
using App.EmailHelper.EmailParameters.Footer;
using App.EmailHelper.EmailParameters.Header;
using App.EmailHelper.EmailTemplates.Body;
using App.EmailHelper.EmailTemplates.Footer;
using App.EmailHelper.EmailTemplates.Header;
using App.EmailHelper.Shared.Enums;
using App.EmailRender.Shared.Abstraction;
using App.EmailRender.Shared.Strategy;

namespace E_BangEmailWorker.Strategy
{
    public class StrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public StrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Dictionary<EEmailFooterType, EmailBuilderMetadata<IEmailTemplate, IEmailParameters>> GenerateEmailFooterStrategy()
        {
            return new Dictionary<EEmailFooterType, EmailBuilderMetadata<IEmailTemplate, IEmailParameters>>
            {
                { EEmailFooterType.Defualt, new EmailBuilderMetadata<IEmailTemplate, IEmailParameters>(_serviceProvider.GetRequiredService<DefaultFooterTemplate>(), new DefaultFooterParameters()) },
                { EEmailFooterType.Custom, null! }
            };
        }

        public Dictionary<EEmailBodyType, EmailBuilderMetadata<IEmailTemplate, IEmailParameters>> GenerateEmailBodyBuilderStrategy()
        {
            return new Dictionary<EEmailBodyType, EmailBuilderMetadata<IEmailTemplate, IEmailParameters>>
            {
                { EEmailBodyType.ConfirmEmail, new EmailBuilderMetadata<IEmailTemplate, IEmailParameters>(_serviceProvider.GetRequiredService<ConfirmEmailTemplate>(), new ConfimEmailParameters()) },
                { EEmailBodyType.Registration, new EmailBuilderMetadata<IEmailTemplate, IEmailParameters>(_serviceProvider.GetRequiredService<RegistrationAccountTemplate>(), new RegistrationAccountParameters()) },
                { EEmailBodyType.TwoWayToken, new EmailBuilderMetadata<IEmailTemplate, IEmailParameters>(_serviceProvider.GetRequiredService<TwoWayTokenTemplate>(), new TwoWayTokenParameters()) },
                { EEmailBodyType.ChangePassword, null! },
            };
        }

        public Dictionary<EEmailHeaderType, EmailBuilderMetadata<IEmailTemplate, IEmailParameters>> GenerateEmailHeaderStrategy()
        {
            return new Dictionary<EEmailHeaderType, EmailBuilderMetadata<IEmailTemplate, IEmailParameters>>
            {
                { EEmailHeaderType.Default, new EmailBuilderMetadata<IEmailTemplate, IEmailParameters>(_serviceProvider.GetRequiredService<DefaultHeaderTemplate>(), new DefaultHeaderParameters()) },
                { EEmailHeaderType.Custom, null! }
            };
        }
    }
}
