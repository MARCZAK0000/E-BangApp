using System.ComponentModel.DataAnnotations;

namespace E_BangNotificationService.OptionsPattern
{
    public class RabbitOptions
    {
        [Required]
        public string Host {  get; set; }

        [Required]
        public string QueueName { get; set; }
    }
}
