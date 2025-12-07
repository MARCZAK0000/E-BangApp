using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangEmailWorker.Exceptions
{
    public class DatabaseNotReacheableException : Exception
    {
        public DatabaseNotReacheableException()
        {
        }

        public DatabaseNotReacheableException(string? message) : base(message)
        {
        }

        public DatabaseNotReacheableException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
