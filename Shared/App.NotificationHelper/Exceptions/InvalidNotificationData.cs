using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.NotificationHelper.Exceptions
{
    public class InvalidNotificationData : Exception
    {
        public InvalidNotificationData(string message) : base(message)
        {
        }
    }
}
