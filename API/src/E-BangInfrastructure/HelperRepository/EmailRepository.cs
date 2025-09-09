using E_BangDomain.HelperRepository;
using E_BangDomain.ModelDtos.MessageSender;
using System.Text.Json;
using App.EmailHelper.EmailParameters.Body;
using App.EmailHelper.EmailParameters.Header;
using App.EmailHelper.EmailParameters.Footer;
using App.RabbitSharedClass.Email;
using App.EmailHelper.Shared.Enums;
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

            EmailComponent<DefaultHeaderParameters, ConfimEmailParameters, DefaultFooterParameters> buildEmail =
                EmailComponent<DefaultHeaderParameters, ConfimEmailParameters, DefaultFooterParameters>.Builder()
                .WithHeader(EEmailHeaderType.Default,defaultHeaderParameters)
                .WithFooter(EEmailFooterType.Defualt, defaultFooterParameters)
                .WithBody(EEmailBodyType.ConfirmEmail,confimEmailParameters)
                .WithSubject("Confirm your email")
                .WithAddressTo(email)
                .Build();


            var rabbitMessageDto = new RabbitMessageBaseDto<EmailComponent<DefaultHeaderParameters, ConfimEmailParameters, DefaultFooterParameters>>()
            {
                Message = buildEmail,
                RabbitChannel = E_BangDomain.Enums.ERabbitChannel.EmailChannel
            };
            await _rabbitSenderRepository.AddMessageToQueue(rabbitMessageDto, cancellationToken);
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

            EmailComponent<DefaultHeaderParameters, RegistrationAccountParameters, DefaultFooterParameters> buildEmail =
                EmailComponent<DefaultHeaderParameters, RegistrationAccountParameters, DefaultFooterParameters>.Builder()
                .WithHeader(EEmailHeaderType.Default, defaultHeaderParameters)
                .WithFooter(EEmailFooterType.Defualt, defaultFooterParameters)
                .WithBody(EEmailBodyType.Registration, confimEmailParameters)
                .WithSubject("Confirm your email")
                .WithAddressTo(email)
                .Build();


            var rabbitMessageDto = new RabbitMessageBaseDto<EmailComponent<DefaultHeaderParameters, RegistrationAccountParameters, DefaultFooterParameters>>()
            {
                Message = buildEmail,
                RabbitChannel = E_BangDomain.Enums.ERabbitChannel.EmailChannel
            };
            await _rabbitSenderRepository.AddMessageToQueue(rabbitMessageDto, cancellationToken);
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

            EmailComponent<DefaultHeaderParameters, TwoWayTokenParameters, DefaultFooterParameters> buildEmail =
                EmailComponent<DefaultHeaderParameters, TwoWayTokenParameters, DefaultFooterParameters>.Builder()
                .WithHeader(EEmailHeaderType.Default, defaultHeaderParameters)
                .WithFooter(EEmailFooterType.Defualt, defaultFooterParameters)
                .WithBody(EEmailBodyType.ConfirmEmail, confimEmailParameters)
                .WithSubject("Confirm your email")
                .WithAddressTo(email)
                .Build();


            var rabbitMessageDto = new RabbitMessageBaseDto<EmailComponent<DefaultHeaderParameters, TwoWayTokenParameters, DefaultFooterParameters>>()
            {
                Message = buildEmail,
                RabbitChannel = E_BangDomain.Enums.ERabbitChannel.EmailChannel
            };
            await _rabbitSenderRepository.AddMessageToQueue(rabbitMessageDto, cancellationToken);
        }
    }
}
