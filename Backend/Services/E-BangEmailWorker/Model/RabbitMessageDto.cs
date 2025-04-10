namespace E_BangEmailWorker.Model
{
    public class RabbitMessageDto
    {
        public string AddressEmail { get; set; }
        public string Subject { get; set; }
        public object MessageStructure { get; set; }
    }
}
