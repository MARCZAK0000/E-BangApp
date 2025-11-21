using App.RabbitSharedClass.UniversalModel;
using System.Text.Json;

namespace App.RabbitSharedClass.Notifications
{
    public class NotificationMessageModel
    {
        public JsonElement Message { get; set; }
        public string AccountId { get; set; } = string.Empty;
        public required ForceNotification ForceNotification { get; set; } 

        public JsonElement ToJsonElement()
        {
            return JsonSerializer.SerializeToElement(this);
        }
    }
}
