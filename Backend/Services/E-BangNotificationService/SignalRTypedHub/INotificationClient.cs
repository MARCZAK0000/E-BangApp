using E_BangAppRabbitSharedClass.RabbitModel;

namespace SignalRTypedHub
{
    public interface INotificationClient
    {
        Task RecivedMessage(NotificationRabbitMessageModel message);
    }
}
