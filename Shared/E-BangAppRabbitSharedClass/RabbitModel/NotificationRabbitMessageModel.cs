namespace E_BangAppRabbitSharedClass.RabbitModel
{
    public class NotificationRabbitMessageModel
    {
        public string ReciverId { get; set; }
        public string ReciverName { get; set; }
        public string Text { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public bool IsReaded { get; set; } = false;
    }
}
