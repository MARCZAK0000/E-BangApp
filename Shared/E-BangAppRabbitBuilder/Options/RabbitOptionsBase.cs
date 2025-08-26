using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangAppRabbitBuilder.Options
{
    public class RabbitOptionsBase
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }

        public override string ToString()
        {
            return $"{nameof(Host)}: {Host}, " +
                   $"{nameof(Port)}: {Port}, " +
                   $"{nameof(UserName)}: {UserName}, " +
                   $"{nameof(Password)}: {Password}, " +
                   $"{nameof(VirtualHost)}: {VirtualHost}, ";
        }
    }
}
