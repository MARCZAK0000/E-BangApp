using App.RabbitSharedClass.UniversalModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace NotificationEntities
{
    [Table("NotificationSettings", Schema = "Account")]
    public class NotificationSettings
    {
        [Key]
        public string AccountId { get; set; }
        public bool IsEmailNotificationEnabled { get; set; } = true;
        public bool IsPushNotificationEnabled { get; set; } = true;
        public bool IsSmsNotificationEnabled { get; set; } = false; 
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }

    public class NotificationSettingsExtensions
    {
        public static NotificationSettings DummySettings(ForceNotification forcedNotificationSettings, string accountId)
        {
            return new NotificationSettings
            {
                AccountId = accountId,
                IsEmailNotificationEnabled = forcedNotificationSettings.ForceEmail,
                IsPushNotificationEnabled = forcedNotificationSettings.ForcePush,
                IsSmsNotificationEnabled = forcedNotificationSettings.ForceSms,
                LastUpdated = DateTime.Now
            };
        }
    }
}
