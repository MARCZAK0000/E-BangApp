using E_BangAppRabbitSharedClass.BuildersDto.Body.BodyBase;
using E_BangAppRabbitSharedClass.BuildersDto.Footer;
using E_BangAppRabbitSharedClass.BuildersDto.Header;

namespace E_BangAppRabbitSharedClass.RabbitMessageChilds
{
    public class EmailBody
    {
        public HeaderDefaultTemplateBuilder Header { get; set; }
        public object Body { get; set; }
        public FooterDefualtTemplateBuilder Footer { get; set; }
    }
}
