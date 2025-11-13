using App.EmailHelper.EmailParameters.Body;
using App.EmailHelper.EmailParameters.Footer;
using App.EmailHelper.EmailParameters.Header;
using App.EmailHelper.Shared.Email;
using App.EmailHelper.Shared.Enums;
using App.RabbitSharedClass.Enum;
using App.RabbitSharedClass.Notifications;
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

        public async Task SendEmailConfirmAccountAsync(string accountID, string token, string email, CancellationToken cancellationToken)
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


            await AddToQueue(accountID, buildEmail);
        }

        public async Task SendForgetPasswordTokenEmailAsync(string accountID, string token, string email, CancellationToken cancellationToken)
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

        public async Task SendRegistrationConfirmAccountEmailAsync(string accountID, string token, string email, CancellationToken cancellationToken)
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

            await AddToQueue(accountID, buildEmail);
        }
        public async Task SendTwoWayTokenEmailAsync(string accountID, string token, string email, CancellationToken cancellationToken)
        {
            TwoWayTokenParameters twoWayTokenParameters = new()
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
                .WithBody(EEmailBodyType.TwoWayToken, twoWayTokenParameters)
                .WithSubject("Confirm your email")
                .WithAddressTo(email)
                .Build()
                .ToEmailComponentJson()
                .ToJsonElement();


            await AddToQueue(accountID, buildEmail);
        }
        private Task<bool> AddToQueue(string accountID, JsonElement message)
        {
            NotificationMessageModel notificationMessageModel = new()
            {
                AccountId = accountID,
                ForceEmail = false,
                ForceNotification = false,
                ForceSms = false,
                Message = message
            };
            var rabbitMessageDto = new RabbitMessageModel()
            {
                Message = notificationMessageModel.ToJsonElement(),
                Channel = ERabbitChannel.NotificationChannel
            };
            return _rabbitSenderRepository.AddMessageToQueue(rabbitMessageDto, CancellationToken.None);
        }
        private Task<bool> AddToQueue(JsonElement message, bool forceEmail, bool forcePush, bool forceSMS)
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
