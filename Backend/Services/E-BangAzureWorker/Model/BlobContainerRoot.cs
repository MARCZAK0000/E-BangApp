namespace E_BangAzureWorker.Model
{
    public class BlobContainerRoot
    {
        public int Id { get; set; } 
        public string RootPath { get; set; }
        public DateTime LastUpdateTime { get; set; }
    }
}
