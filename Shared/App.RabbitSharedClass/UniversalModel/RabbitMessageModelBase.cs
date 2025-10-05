using App.RabbitSharedClass.Email;
using App.RabbitSharedClass.Enum;
using App.RabbitSharedClass.Notifications;
using System.Text.Json;
using System.Threading.Channels;

namespace App.RabbitSharedClass.UniModel
{
    public class RabbitMessageModelBase
    {
        public JsonElement Message { get; set; }

        public virtual EmailComponentMessage ToEmail()
        {
            
            return new EmailComponentMessage
            {
                EmailMessage = Message
            };
        }

        public virtual PushNotificationMessageModel ToPush()
        {
            return new PushNotificationMessageModel
            {
                PushMessage = Message
            };
        }
    }
}
