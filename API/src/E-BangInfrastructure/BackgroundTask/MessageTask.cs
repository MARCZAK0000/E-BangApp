using System.Runtime.CompilerServices;
using E_BangApplication.Exceptions;
using E_BangDomain.BackgroundTask;
using E_BangDomain.ModelDtos.MessageSender;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using E_BangAppRabbitBuilder.Service.Sender;
using E_BangAppRabbitBuilder.Options;

namespace E_BangInfrastructure.BackgroundTask
{
    public class MessageTask : IMessageTask
    {
        private readonly ILogger<MessageTask> _logger;

        private readonly IRabbitSenderService _rabbitSenderService;

        private readonly RabbitOptions _rabbitOptions;
        public MessageTask(ILogger<MessageTask> logger, IRabbitSenderService rabbitSenderService, RabbitOptions rabbitOptions)
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
        public async Task SendToRabbitChannelAsync<T>(RabbitMessageBaseDto<T> parameters, CancellationToken token) where T : class
        {
            try
            {
                _rabbitOptions.SenderQueueName = Enum.GetName(parameters.RabbitChannel)!;
                _logger.LogInformation("Message Task: Add to Queue at {Date}, Message Type {type}", DateTime.Now, typeof(T).Name);
                await _rabbitSenderService.InitSenderRabbitQueueAsync(_rabbitOptions, parameters.Message);
            }
            catch (Exception e)
            {
                _logger.LogError("Message Task: Error at {date}: {e}", DateTime.Now, e);
                throw;
            }   
        }


    }
}
