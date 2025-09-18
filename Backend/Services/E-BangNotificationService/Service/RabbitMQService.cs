using E_BangAppRabbitBuilder.Options;
using E_BangAppRabbitBuilder.Service.Listener;
using E_BangAppRabbitSharedClass.RabbitModel;
using E_BangNotificationService.NotificationEntities;
using E_BangNotificationService.Repository;
using Microsoft.AspNetCore.SignalR;
using SignalRHub;
using SignalRTypedHub;

namespace E_BangNotificationService.Service
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly ILogger<RabbitMQService> _logger;

        private readonly RabbitOptions _rabbitOptions;

        private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;

        private readonly IDbRepository _dbRepo;

        private readonly IRabbitListenerService _rabbitListenerService;

        public RabbitMQService(ILogger<RabbitMQService> logger,
            RabbitOptions rabbitOptions,
            IHubContext<NotificationHub,
            INotificationClient> hubContext,
            IDbRepository dbRepo,
            IRabbitListenerService rabbitListenerService)
        {
            _logger = logger;
            _rabbitOptions = rabbitOptions;
            _hubContext = hubContext;
            _dbRepo = dbRepo;
            _rabbitListenerService = rabbitListenerService;
        }

        public async Task CreateListenerQueueAsync(CancellationToken token)
        {
            await _rabbitListenerService.InitListenerRabbitQueueAsync(_rabbitOptions, async (NotificationRabbitMessageModel messageModel) =>
            {
                Notifcation notifcation = new Notifcation()
                {
                    ReciverId = messageModel.ReciverId,
                    ReciverName = messageModel.ReciverName,
                    SenderId = messageModel.SenderId,
                    SenderName = messageModel.SenderName,
                    IsReaded = messageModel.IsReaded,
                    Text = messageModel.Text,
                    LastUpdateTime = DateTime.Now,
                };
                bool IsSaved = await _dbRepo.SaveNotificationAsync(notifcation, token);
                if (IsSaved)
                {
                    await _hubContext.Clients.Client(messageModel.ReciverId).RecivedMessage(messageModel);
                    _logger.LogInformation("Notification send at {DateTime} from AccountID: {Id} to AccountId{Id2}", DateTime.Now, messageModel.SenderId, messageModel!.ReciverId);
                }
                else
                {
                    _logger.LogError("Notification not send from AccountID: {Id} to AccountId{Id2}", messageModel.SenderId, messageModel!.ReciverId);
                }
            }, token);

        }
    }
}
