using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.Service.Listener;
using Microsoft.AspNetCore.SignalR;
using NotificationEntities;
using SignalRHub;
using SignalRTypedHub;

namespace Service
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly NotificationDbContext _notificationDbContext;

        private readonly ILogger<RabbitMQService> _logger;

        private readonly RabbitOptions _rabbitOptions;

        private readonly IRabbitListenerService _rabbitListenerService;

        public RabbitMQService(ILogger<RabbitMQService> logger,
            RabbitOptions rabbitOptions,
            IHubContext<NotificationHub,
            INotificationClient> hubContext,
            IRabbitListenerService rabbitListenerService,
            NotificationDbContext notificationDbContext)
        {
            _logger = logger;
            _rabbitOptions = rabbitOptions;
            _rabbitListenerService = rabbitListenerService;
            _notificationDbContext = notificationDbContext;
        }

        public async Task<bool> ListenerQueueAsync(CancellationToken token)
        {
            await _rabbitListenerService.InitListenerRabbitQueueAsync(_rabbitOptions, async (NotificationRabbitMessageModel messageModel) =>
            {


            }, token);
            return true;
        }
    }
}
