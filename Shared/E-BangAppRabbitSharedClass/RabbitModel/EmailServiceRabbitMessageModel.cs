using E_BangAppRabbitSharedClass.RabbitMessageChilds;

namespace E_BangAppRabbitSharedClass.RabbitModel
{
    public class EmailServiceRabbitMessageModel
    {
        public string AddressTo { get; set; }
        public string Subject { get; set; }
        public EmailBody Body { get; set; }
    }
}
