using E_BangAppEmailBuilder.src.BuildersDto.Body;

namespace E_BangAppRabbitSharedClass.RabbitModel
{
    public class EmailServiceRabbitMessageModel
    {
        public string AddressTo { get; set; }
        public string Subject { get; set; }
        public EmailBodyBuilderBase Body { get; set; }
    }

}
