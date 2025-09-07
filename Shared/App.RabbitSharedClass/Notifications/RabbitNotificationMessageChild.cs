using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangAppRabbitSharedClass.Notifications
{
    public class RabbitNotificationMessageChild
    {
        public string ReciverId { get; set; }
        public string ReciverName { get; set; }
        public string Text { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public bool IsReaded { get; set; } = false;
    }
}
