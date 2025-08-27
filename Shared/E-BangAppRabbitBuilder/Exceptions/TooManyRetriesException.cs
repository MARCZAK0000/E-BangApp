using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangAppRabbitBuilder.Exceptions
{
    public class TooManyRetriesException : Exception
    {
        public TooManyRetriesException()
        {
        }

        public TooManyRetriesException(string? message) : base(message)
        {
        }
    }
}
