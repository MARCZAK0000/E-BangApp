using System;
using System.Collections.Generic;

namespace E_BangInfrastructure.NotificationsEntity;

public partial class Notifcation
{
    public string NotificationId { get; set; } = null!;

    public string ReciverId { get; set; } = null!;

    public string ReciverName { get; set; } = null!;

    public string Text { get; set; } = null!;

    public string SenderId { get; set; } = null!;

    public string SenderName { get; set; } = null!;

    public bool IsReaded { get; set; }

    public DateTime LastUpdateTime { get; set; }
}
