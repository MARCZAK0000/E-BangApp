using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangAppRabbitSharedClass.Exceptions
{
    public class EmailTemplatedNotFoundException : Exception
    {
        public EmailTemplatedNotFoundException()
        {
        }

        public EmailTemplatedNotFoundException(string? message) : base(message)
        {
        }
    }
}
