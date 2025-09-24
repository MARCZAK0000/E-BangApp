using System.Text.Json;

namespace App.RabbitSharedClass.Notifications
{
    public class NotificationMessageModel
    {
        public JsonElement Message { get; set; }
        public string AccountId { get; set; } = string.Empty;
        public bool ForceEmail { get; set; } = false;
        public bool ForceNotification { get; set; } = false;
        public bool ForceSms { get; set; } = false;

        public JsonElement ToJsonElement()
        {
            return JsonSerializer.SerializeToElement(this);
        }
    }
}
