namespace E_BangAzureWorker.Model
{
    public class BlobItems
    {
        public int BlobItemId { get; set; }
        public string BlobItemName { get; set; }
        public string BlobItemType { get; set; }
        public string? ProductId { get; set; }
        public string? AccountId { get; set; }
        public int ContainerID { get; set; }
        public DateTime LastUpdateTime { get; set; }
        public BlobContainer Container { get; set; }

    }
}
