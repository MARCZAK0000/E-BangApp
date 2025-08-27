using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangAppRabbitBuilder.Configuration
{
    public class ConfigurationOptions
    {
        public int ConnectionRetryCount { get; set; } = 2;
        public int ConnectionRetryDelaySeconds { get; set; } = 1;
        public int ServiceRetryCount { get; set; } = 5;
        public int ServiceRetryDelaySeconds { get; set; } = 2;

        public override string ToString()
        {
            return $"ConnectionRetryCount: {ConnectionRetryCount}, " +
                $"ConnectionRetryDelaySeconds: {ConnectionRetryDelaySeconds}, " +
                $"ServiceRetryCount: {ServiceRetryCount}, " +
                $"ServiceRetryDelaySeconds: {ServiceRetryDelaySeconds}";
        }
    }
}
