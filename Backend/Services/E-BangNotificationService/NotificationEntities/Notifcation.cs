namespace E_BangNotificationService.NotificationEntities
{
    public class Notifcation
    {
        public string NotificationId { get; set; } = Guid.NewGuid().ToString();   
        public string ReciverId { get; set; }
        public string ReciverName { get; set; }
        public string Text { get; set; } 
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public bool IsReaded { get; set; } = false;
        public DateTime LastUpdateTime { get; set; }

    }
}
