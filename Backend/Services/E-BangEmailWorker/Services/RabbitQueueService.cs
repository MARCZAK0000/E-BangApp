using App.EmailBuilder.Service;
using App.EmailHelper.Shared.Email;
using App.RabbitSharedClass.Email;
using App.RenderEmail.RenderEmail;
using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.Service.Listener;
using E_BangEmailWorker.Exceptions;
using E_BangEmailWorker.Model;
using E_BangEmailWorker.Repository;
using E_BangEmailWorker.Strategy;
using MimeKit;
using System.Text.Json;
namespace E_BangEmailWorker.Services
{
    public class RabbitQueueService : IRabbitQueueService
    {

        private readonly IMessageRepository _messageRepository;

        private readonly IDatabaseRepository _databaseRepository;

        private readonly RabbitOptions _rabbitOptions;

        private readonly RenderEmailBuilder _renderEmailBuilder;

        private readonly ILogger<RabbitQueueService> _logger;

        private readonly IRabbitListenerService _rabbitListenerService;

        private readonly IEmailSenderService _emailSenderService;

        private readonly StrategyFactory _strategyFactory;
        public RabbitQueueService(
            IDatabaseRepository databaseRepository,
            RabbitOptions rabbitOptions,
            ILogger<RabbitQueueService> logger,
            RenderEmailBuilder renderEmailBuilder,
            IMessageRepository messageRepository,
            IEmailSenderService emailSenderService,
            IRabbitListenerService rabbitListenerService,
            StrategyFactory strategyFactory)
        {
            _emailSenderService = emailSenderService;
            _databaseRepository = databaseRepository;
            _rabbitOptions = rabbitOptions;
            _logger = logger;
            _renderEmailBuilder = renderEmailBuilder;
            _messageRepository = messageRepository;
            _rabbitListenerService = rabbitListenerService;
            _strategyFactory = strategyFactory;

        }
        public async Task HandleRabbitQueueAsync(CancellationToken token)
        {
            _logger.LogInformation("{Date} - Email Rabbit Queue: Rabbit Options: {userName}, {password}, {host}", DateTime.Now, _rabbitOptions.UserName, _rabbitOptions.Password, _rabbitOptions.Host);
            await _rabbitListenerService.InitListenerRabbitQueueAsync(rabbitOptions: _rabbitOptions, async (EmailComponentMessage messageModel) =>
            {

                var headerDictionary = _strategyFactory.GenerateEmailHeaderStrategy();
                var bodyDictionary = _strategyFactory.GenerateEmailBodyBuilderStrategy();
                var footerDictionary = _strategyFactory.GenerateEmailFooterStrategy();
                EmailComponentJson emailComponent = JsonSerializer.Deserialize<EmailComponentJson>(messageModel.EmailComponentsJson)??throw new Exception("XD");
                var headerResult = await _renderEmailBuilder.SetHeader(emailComponent.HeaderType, headerDictionary, JsonSerializer.SerializeToElement(emailComponent.HeaderParameters));
                var bodyResult = await headerResult.SetBody(emailComponent.BodyType, bodyDictionary, JsonSerializer.SerializeToElement(emailComponent.BodyParameters));
                var footerResult = await bodyResult.SetFooter(emailComponent.FooterType, footerDictionary, JsonSerializer.SerializeToElement(emailComponent.FooterParameters));
                EmailMessage emailRawHTML = footerResult.Build();

                MimeMessage mimeMessage = _messageRepository.BuildMessage(new SendMailDto(emailComponent.AddressTo, emailRawHTML.Message, emailComponent.Subject), token);
                _logger.LogInformation("{Date} - Email Rabbit Queue: Saving email info: FROM -> {mimeMessage.From} TO-> {mimeMessage.To}", DateTime.Now, mimeMessage.From, mimeMessage.To);
                bool isSend = await _emailSenderService.SendEmailAsync(mimeMessage, token);
                await _databaseRepository.SaveEmailInfo(emailComponent.AddressTo, isSend, token);
                if (!isSend)
                {
                    _logger.LogError("{Date} - Email Rabbit Queue: There is a problem with email message: {messageModel}", DateTime.Now, messageModel);
                    throw new MesseageNotSendException("Email Message Problem");
                }
                _logger.LogInformation("{Date} - Email Rabbit Queue: Sending email: FROM -> {mimeMessage.From} TO-> {mimeMessage.To}", DateTime.Now, mimeMessage.From, mimeMessage.To);
            }, token);
        }
    }
}
