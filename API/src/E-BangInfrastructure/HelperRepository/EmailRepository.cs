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


            await GenerateMessage(accountID, buildEmail, ForceNotificationDefaults.EmailOnly);
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

            await GenerateMessage(accountID, buildEmail, ForceNotificationDefaults.EmailOnly);
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

            ForceNotification allowEmailAndSms = new()
            {
                ForceEmail = true,
                ForceSms = true
            };

            await GenerateMessage(accountID, buildEmail, allowEmailAndSms);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountID"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private Task<bool> GenerateMessage(string accountID, JsonElement message)
        {
            NotificationMessageModel notificationMessageModel = new()
            {
                AccountId = accountID,
                ForceNotification = ForceNotificationDefaults.None,
                Message = message
            };

            RabbitMessageModel rabbitMessageDto = new()
            {
                Message = notificationMessageModel.ToJsonElement(),
                Channel = ERabbitChannel.NotificationChannel
            };

            return AddToQueue(rabbitMessageDto);
        }
        /// <summary>
        /// Generates and enqueues a notification message for the specified account.
        /// </summary>
        /// <remarks>This method creates a notification message using the provided parameters and enqueues
        /// it for processing. The message is sent to the notification channel for further handling.</remarks>
        /// <param name="accountId">The unique identifier of the account for which the notification is generated. Cannot be <see
        /// langword="null"/> or empty.</param>
        /// <param name="message">The content of the notification message, represented as a JSON element.</param>
        /// <param name="forceNotification">Specifies whether the notification should bypass standard delivery rules and be sent immediately.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is <see langword="true"/> if the message
        /// was successfully added to the queue; otherwise, <see langword="false"/>.</returns>
        private Task<bool> GenerateMessage(string accountId, JsonElement message, ForceNotification forceNotification)
        {
           NotificationMessageModel notificationMessageModel = new()
            {
                AccountId = accountId,
                ForceNotification = forceNotification,
                Message = message
            };

            RabbitMessageModel rabbitMessageDto = new()
            {
                Message = notificationMessageModel.ToJsonElement(),
                Channel = ERabbitChannel.NotificationChannel
            };
            return AddToQueue(rabbitMessageDto);
        }

        private Task<bool> AddToQueue(RabbitMessageModel rabbitMessageDto)
        {
            return _rabbitSenderRepository.AddMessageToQueue(rabbitMessageDto, CancellationToken.None);
        }
    }
}
