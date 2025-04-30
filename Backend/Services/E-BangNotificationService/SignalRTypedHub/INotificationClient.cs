using E_BangAppRabbitSharedClass.RabbitModel;

namespace E_BangNotificationService.SignalRTypedHub
{
    public interface INotificationClient
    {
        Task RecivedMessage(NotificationRabbitMessageModel message);
    }
}
