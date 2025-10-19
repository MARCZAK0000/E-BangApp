using App.EmailHelper.EmailParameters.Body;
using App.EmailHelper.EmailParameters.Footer;
using App.EmailHelper.EmailParameters.Header;
using App.EmailHelper.Shared.Email;
using App.EmailHelper.Shared.Enums;
using App.RabbitSharedClass.Enum;
using App.RabbitSharedClass.UniversalModel;
using E_BangDomain.HelperRepository;
using System.Text.Json;
namespace E_BangInfrastructure.HelperRepository
{
    public class EmailRepository : IEmailRepository
    {
        private readonly IRabbitSenderRepository _rabbitSenderRepository;

        public EmailRepository(IRabbitSenderRepository rabbitSenderRepository)
        {
            _rabbitSenderRepository = rabbitSenderRepository;
        }

        public async Task SendEmailConfirmAccountAsync(string token, string email, CancellationToken cancellationToken)
        {

            ConfimEmailParameters confimEmailParameters = new()
            {
                Email = email,
                Token = token
            };
            DefaultHeaderParameters defaultHeaderParameters = new()
            {
                AppName = "E-Bang",
            };
            DefaultFooterParameters defaultFooterParameters = new()
            {
                Year = DateTime.Now.Year.ToString(),
                AppName = "E-Bang"
            };

            JsonElement buildEmail =
                EmailComponent<DefaultHeaderParameters, ConfimEmailParameters, DefaultFooterParameters>.Builder()
                .WithHeader(EEmailHeaderType.Default, defaultHeaderParameters)
                .WithFooter(EEmailFooterType.Defualt, defaultFooterParameters)
                .WithBody(EEmailBodyType.ConfirmEmail, confimEmailParameters)
                .WithSubject("Confirm your email")
                .WithAddressTo(email)
                .Build()
                .ToEmailComponentJson()
                .ToJsonElement();


            await AddToQueue(buildEmail);
        }

        public async Task SendForgetPasswordTokenEmailAsync(string token, string email, CancellationToken cancellationToken)
        {
            //var buildEmail = new EmailBody()
            //{
            //    AdressedTo = email,
            //    Subject = "Forget Password Token",
            //    Header = new HeaderDefaultTemplateBuilder
            //    {
            //        Email = email,
            //    },
            //    Footer = new FooterDefualtTemplateBuilder()
            //    {
            //        Year = DateTime.Now.Year.ToString(),
            //    }
            //};
            //var rabbitMessageDto = new RabbitMessageBaseDto<EmailServiceRabbitMessageModel>()
            //{
            //    Message = new EmailServiceRabbitMessageModel()
            //    {
            //        EEnumEmailBodyBuilderType = EEnumEmailBodyBuilderType.Registration,
            //        Message = JsonSerializer.SerializeToElement(buildEmail)
            //    },
            //    RabbitChannel = E_BangDomain.Enums.ERabbitChannel.EmailChannel
            //};
            //await _rabbitSenderRepository.AddMessageToQueue(rabbitMessageDto, cancellationToken);
            throw new NotImplementedException();
        }

        public async Task SendRegistrationConfirmAccountEmailAsync(string token, string email, CancellationToken cancellationToken)
        {
            RegistrationAccountParameters confimEmailParameters = new()
            {
                Email = email,
                Token = token
            };
            DefaultHeaderParameters defaultHeaderParameters = new()
            {
                AppName = "E-Bang",
            };
            DefaultFooterParameters defaultFooterParameters = new()
            {
                Year = DateTime.Now.Year.ToString(),
                AppName = "E-Bang"
            };

            JsonElement buildEmail =
                EmailComponent<DefaultHeaderParameters, RegistrationAccountParameters, DefaultFooterParameters>.Builder()
                .WithHeader(EEmailHeaderType.Default, defaultHeaderParameters)
                .WithFooter(EEmailFooterType.Defualt, defaultFooterParameters)
                .WithBody(EEmailBodyType.Registration, confimEmailParameters)
                .WithSubject("Confirm your email")
                .WithAddressTo(email)
                .Build()
                .ToEmailComponentJson()
                .ToJsonElement();

            await AddToQueue(buildEmail);
        }
        public async Task SendTwoWayTokenEmailAsync(string token, string email, CancellationToken cancellationToken)
        {
            TwoWayTokenParameters confimEmailParameters = new()
            {
                Email = email,
                Token = token
            };
            DefaultHeaderParameters defaultHeaderParameters = new()
            {
                AppName = "E-Bang",
            };
            DefaultFooterParameters defaultFooterParameters = new()
            {
                Year = DateTime.Now.Year.ToString(),
                AppName = "E-Bang"
            };

            JsonElement buildEmail =
                EmailComponent<DefaultHeaderParameters, TwoWayTokenParameters, DefaultFooterParameters>.Builder()
                .WithHeader(EEmailHeaderType.Default, defaultHeaderParameters)
                .WithFooter(EEmailFooterType.Defualt, defaultFooterParameters)
                .WithBody(EEmailBodyType.ConfirmEmail, confimEmailParameters)
                .WithSubject("Confirm your email")
                .WithAddressTo(email)
                .Build()
                .ToEmailComponentJson()
                .ToJsonElement();


            await AddToQueue(buildEmail);
        }
        private Task<bool> AddToQueue(JsonElement message)
        {
            var rabbitMessageDto = new RabbitMessageModel()
            {
                Message = message,
                Channel = ERabbitChannel.NotificationChannel
            };
            return _rabbitSenderRepository.AddMessageToQueue(rabbitMessageDto, CancellationToken.None);
        }
    }
}
