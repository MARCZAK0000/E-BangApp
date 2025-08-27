using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangAppRabbitBuilder.Options
{
    public class QueueOptions
    {
        public string Name { get; set; }
        public string QueueName { get; set; }

        public override string ToString()
        {
            return $"{nameof(Name)}: {Name}, " +
                   $"{nameof(QueueName)}: {QueueName}";
        }
    }
    
}
