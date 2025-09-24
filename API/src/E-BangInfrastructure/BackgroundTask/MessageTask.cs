using App.RabbitBuilder.Options;
using App.RabbitBuilder.Service.Sender;
using App.RabbitSharedClass.UniversalModel;
using E_BangDomain.BackgroundTask;
using E_BangDomain.Dictionary;
using Microsoft.Extensions.Logging;

namespace E_BangInfrastructure.BackgroundTask
{
    public class MessageTask : IMessageTask
    {
        private readonly ILogger<MessageTask> _logger;

        private readonly IRabbitSenderService _rabbitSenderService;

        private readonly RabbitOptionsExtended _rabbitOptions;
        public MessageTask(ILogger<MessageTask> logger, IRabbitSenderService rabbitSenderService, RabbitOptionsExtended rabbitOptions)
        {
            _logger = logger;
            _rabbitSenderService = rabbitSenderService;
            _rabbitOptions = rabbitOptions;
        }

        /// <summary>
        /// Send Message to RabbitMQ
        /// </summary>
        /// <param name="parameters">Message Parameters</param>
        /// <param name="token">Cancelation Token</param>
        /// <returns></returns>
        public async Task SendToRabbitChannelAsync<T>(RabbitMessageModel<T> parameters, CancellationToken token) where T : class
        {
            string queueName = RabbitChannelDictionary.RabbitChannelName.Where(pr => pr.Key == parameters.Channel)
                .Select(pr => pr.Value).FirstOrDefault() ?? throw new ArgumentNullException($"Queue with channel '{parameters.Channel}' not found in SenderQueues.");
            try
            {
                _logger.LogInformation("Message Task: Add to Queue at {Date}, Message Type {type}, Message Queue: {queue}",
                    DateTime.Now, typeof(T).Name, _rabbitOptions.SenderQueues?.ToString());
                await _rabbitSenderService.InitSenderRabbitQueueAsync(_rabbitOptions, parameters.Message, queueName, token);
            }
            catch (Exception e)
            {
                _logger.LogError("Message Task: Error at {date}: {e} - {stackTrace}", DateTime.Now, e.Message, e.StackTrace);
                throw;
            }
        }


    }
}
