using E_BangAppEmailBuilder.src.Abstraction;
using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.Service.Listener;
using E_BangAppRabbitSharedClass.RabbitModel;
using E_BangEmailWorker.Exceptions;
using E_BangEmailWorker.Model;
using E_BangEmailWorker.OptionsPattern;
using E_BangEmailWorker.Repository;
using MimeKit;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
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

            await _rabbitListenerService.InitListenerRabbitQueueAsync(rabbitOptions: _rabbitOptions, async (EmailServiceRabbitMessageModel messageModel) =>
            {
                string emailRawHTML = _builderEmail
                    .GenerateMessage(messageModel.Body.Header, messageModel.Body.Body, messageModel.Body.Footer)
                    .Message;
                MimeMessage mimeMessage = _messageRepository.BuildMessage(new SendMailDto(messageModel.AddressTo, emailRawHTML, messageModel.Subject), token);
                bool isSend = await _emailRepository.SendEmailAsync(mimeMessage, token);
                if (!isSend)
                {
                    _logger.LogError("There is a problem with email message: {messageModel}", messageModel);
                    throw new MesseageNotSendException("Email Message Problem");
                }
                _logger.LogInformation("Sending email: FROM -> {mimeMessage.From} TO-> {mimeMessage.To}", mimeMessage.From, mimeMessage.To);
                await _databaseRepository.SaveEmailInfo(messageModel, token);
                _logger.LogInformation("Saving emial info: FROM -> {mimeMessage.From} TO-> {mimeMessage.To}", mimeMessage.From, mimeMessage.To);
            }, token);
        }
    }
}
