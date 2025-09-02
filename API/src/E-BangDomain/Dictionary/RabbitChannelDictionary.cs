using E_BangDomain.Enums;

namespace E_BangDomain.Dictionary
{
    public static class RabbitChannelDictionary
    {
        public static readonly Dictionary<ERabbitChannel, string> RabbitChannelName = new Dictionary<ERabbitChannel, string>
            {
                { ERabbitChannel.EmailChannel, "Email" },
                { ERabbitChannel.AzureChannel, "Azure" },
                { ERabbitChannel.NotificationChannel, "Notification" }
            };
    }
}
