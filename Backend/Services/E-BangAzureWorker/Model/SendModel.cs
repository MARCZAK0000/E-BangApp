using Microsoft.Identity.Client;

namespace E_BangAzureWorker.Model
{
    public class SendModel
    {
        public string AccountID { get; set; }

        public string Message { get; set; }

        public string MessageType { get; set; } = "ImageNotification";

    }
}
