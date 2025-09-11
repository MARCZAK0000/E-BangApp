using App.EmailHelper.EmailTemplates.Body;
using App.EmailHelper.EmailTemplates.Footer;
using App.EmailHelper.EmailTemplates.Header;
using App.EmailHelper.Shared.Enums;
using App.EmailRender.Shared.Abstraction;

namespace E_BangEmailWorker.Strategy
{
    public class StrategyFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public StrategyFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Dictionary<EEmailFooterType, IEmailTemplate> GenerateEmailFooterStrategy()
        {
            return new Dictionary<EEmailFooterType, IEmailTemplate>
            {
                { EEmailFooterType.Defualt, _serviceProvider.GetRequiredService<DefaultFooterTemplate>() },
                { EEmailFooterType.Custom, null! }
            };
        }

        public Dictionary<EEmailBodyType, IEmailTemplate> GenerateEmailBodyBuilderStrategy()
        {
            return new Dictionary<EEmailBodyType, IEmailTemplate>
            {
                { EEmailBodyType.ConfirmEmail, _serviceProvider.GetRequiredService<ConfirmEmailTemplate>() },
                { EEmailBodyType.Registration, _serviceProvider.GetRequiredService<RegistrationAccountTemplate>() },
                { EEmailBodyType.TwoWayToken, _serviceProvider.GetRequiredService<TwoWayTokenTemplate>() },
                { EEmailBodyType.ChangePassword, null! },
            };
        }

        public Dictionary<EEmailHeaderType, IEmailTemplate> GenerateEmailHeaderStrategy()
        {
            return new Dictionary<EEmailHeaderType, IEmailTemplate>
            {
                { EEmailHeaderType.Default, _serviceProvider.GetRequiredService<DefaultHeaderTemplate>() },
                { EEmailHeaderType.Custom, null! }
            };
        }
    }
}
