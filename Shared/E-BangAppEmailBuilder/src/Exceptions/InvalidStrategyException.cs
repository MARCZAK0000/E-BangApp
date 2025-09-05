using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangAppEmailBuilder.src.Exceptions
{
    public class InvalidStrategyException : Exception
    {
        public InvalidStrategyException()
        {
        }

        public InvalidStrategyException(string? message) : base(message)
        {
        }
    }
}
