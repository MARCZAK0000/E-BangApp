using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangAppRabbitSharedClass.Exceptions
{
    public class InvalidDataSerializationException : Exception
    {
        public InvalidDataSerializationException()
        {
        }

        public InvalidDataSerializationException(string? message) : base(message)
        {
        }
    }
}
