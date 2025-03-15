namespace E_BangAzureWorker.EventPublisher
{
    public class EventMessageArgs : EventArgs
    {
        public string AccountID { get; set; }

        public string EmailID { get; set; }
    }
}
