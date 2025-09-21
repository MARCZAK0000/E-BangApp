using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
}
