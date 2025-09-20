using App.NotificationHelper.Enums;
using System.Text.Json;

namespace App.NotificationHelper.Message
{
    public class NotificationDataMessage
    {
        public NotificationDataMessage(string title, string text, 
            AccountNotificationData? senderData, AccountNotificationData? receiverData, List<AccountNotificationData>? reciversDataList,
            ENotificationReciver eNotificationRecivers, ENotificationType eNotificationType, 
            DateTime notificationDate)
        {
            Title = title;
            Text = text;
            SenderData = senderData;
            ReceiverData = receiverData;
            ENotificationRecivers = eNotificationRecivers;
            ENotificationType = eNotificationType;
            NotificationDate = notificationDate;
            ReciversDataList = reciversDataList;
        }

        public string Title { get;  }
        public string Text { get;  }

        public AccountNotificationData? SenderData { get; }
        public AccountNotificationData? ReceiverData { get; }
        public ENotificationReciver ENotificationRecivers { get; }
        public ENotificationType ENotificationType { get; }
        public DateTime NotificationDate { get; }
        public List<AccountNotificationData>? ReciversDataList { get;  }

        public static NotificationDataBuilder Builder()
        {
            return new NotificationDataBuilder();
        }

        public JsonElement ToJsonElement()
        {
            return JsonSerializer.SerializeToElement(this);
        }
        public override string ToString()
        {
            return $"NotificationDataMessage {{ Title: '{Title}', Text: '{Text}', Type: {ENotificationType}, " +
               ", Date: {NotificationDate:yyyy-MM-dd HH:mm:ss} }}";
        }
    }
}
