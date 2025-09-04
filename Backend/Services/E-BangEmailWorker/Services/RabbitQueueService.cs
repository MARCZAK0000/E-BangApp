using E_BangAppEmailBuilder.src.Abstraction;
using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.Service.Listener;
using E_BangAppRabbitSharedClass.BuildersDto.RabbitMessageChilds;
using E_BangAppRabbitSharedClass.RabbitModel;
using E_BangEmailWorker.Exceptions;
using E_BangEmailWorker.Model;
using E_BangEmailWorker.Repository;
using MimeKit;
using System.Text.Json;
namespace E_BangEmailWorker.Services
{
    public class RabbitQueueService : IRabbitQueueService
    {
        private readonly IEmailRepository _emailRepository;

        private readonly IMessageRepository _messageRepository;

        private readonly IDatabaseRepository _databaseRepository;

        private readonly RabbitOptions _rabbitOptions;

        private readonly IBuilderEmail _builderEmail;

        private readonly ILogger<RabbitQueueService> _logger;

        private readonly IRabbitListenerService _rabbitListenerService;
        public RabbitQueueService(
            IEmailRepository emailRepository,
            IDatabaseRepository databaseRepository,
            RabbitOptions rabbitOptions,
            ILogger<RabbitQueueService> logger,
            IBuilderEmail builderEmail,
            IMessageRepository messageRepository,
            IRabbitListenerService rabbitListenerService)
        {
            _emailRepository = emailRepository;
            _databaseRepository = databaseRepository;
            _rabbitOptions = rabbitOptions;
            _logger = logger;
            _builderEmail = builderEmail;
            _messageRepository = messageRepository;
            _rabbitListenerService = rabbitListenerService;

        }
        public async Task HandleRabbitQueueAsync(CancellationToken token)
        {
            _logger.LogInformation("{Date} - Email Rabbit Queue: Rabbit Options: {userName}, {password}, {host}", DateTime.Now, _rabbitOptions.UserName, _rabbitOptions.Password, _rabbitOptions.Host);
            await _rabbitListenerService.InitListenerRabbitQueueAsync(rabbitOptions: _rabbitOptions, async (EmailServiceRabbitMessageModel messageModel) =>
            {
                EmailBody emailBody = JsonSerializer.Deserialize<EmailBody>(messageModel.Message) ?? throw new InvalidDataException("Invalid Data in json deserialize to EmailBody");
                string emailRawHTML = _builderEmail
                    .GenerateMessage(emailBody.Header, (messageModel.EEnumEmailBodyBuilderType,emailBody.Body), emailBody.Footer)
                    .Message;
                MimeMessage mimeMessage = _messageRepository.BuildMessage(new SendMailDto(emailBody.AdressedTo, emailRawHTML, emailBody.Subject), token);
                bool isSend = await _emailRepository.SendEmailAsync(mimeMessage, token);
                if (!isSend)
                {
                    _logger.LogError("{Date} - Email Rabbit Queue: There is a problem with email message: {messageModel}", DateTime.Now, messageModel);
                    throw new MesseageNotSendException("Email Message Problem");
                }
                _logger.LogInformation("{Date} - Email Rabbit Queue: Sending email: FROM -> {mimeMessage.From} TO-> {mimeMessage.To}", DateTime.Now, mimeMessage.From, mimeMessage.To);
                await _databaseRepository.SaveEmailInfo(emailBody, token);
                _logger.LogInformation("{Date} - Email Rabbit Queue: Saving email info: FROM -> {mimeMessage.From} TO-> {mimeMessage.To}", DateTime.Now, mimeMessage.From, mimeMessage.To);
            }, token);
        }
    }
}
