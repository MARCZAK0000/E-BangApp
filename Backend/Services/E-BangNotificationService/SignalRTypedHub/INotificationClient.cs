namespace E_BangNotificationService.SignalRTypedHub
{
    public interface INotificationClient
    {
        Task RecivedMessage(string message);
    }
}
