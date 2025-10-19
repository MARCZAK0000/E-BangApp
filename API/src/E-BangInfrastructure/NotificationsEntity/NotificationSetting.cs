using System;
using System.Collections.Generic;

namespace E_BangInfrastructure.NotificationsEntity;

public partial class NotificationSetting
{
    public string AccountId { get; set; } = null!;

    public bool IsEmailNotificationEnabled { get; set; }

    public bool IsPushNotificationEnabled { get; set; }

    public bool IsSmsNotificationEnabled { get; set; }

    public DateTime LastUpdated { get; set; }
}
