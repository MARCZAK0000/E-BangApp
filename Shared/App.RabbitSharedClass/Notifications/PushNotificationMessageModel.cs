using System.Text.Json;

namespace App.RabbitSharedClass.Notifications
{
    public class PushNotificationMessageModel
    {
        public JsonElement NotificationMesseage { get; set; }
    }
}
