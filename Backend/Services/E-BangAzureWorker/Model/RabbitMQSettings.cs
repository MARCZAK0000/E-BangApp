using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangAzureWorker.Model
{
    public interface IRabbitMQSettings
    {
        string Host { get; set; }
        string QueueName { get; set; }
    }
    public class RabbitMQSettings : IRabbitMQSettings
    {
        public string Host { get; set; }
        public string QueueName { get; set; }
    }
}
