using E_BangDomain.Enums;

namespace E_BangDomain.ModelDtos.MessageSender
{
    public class RabbitMessageBaseDto<T> where T : class
    {
        public ERabbitChannel RabbitChannel { get; set; }
        public T Message { get; set; }
    }
}
