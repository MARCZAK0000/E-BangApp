namespace E_BangAzureWorker.Model
{
    public class FileChangesInformations
    {
        public int ContainerId { get; set; }
        public string AccountID { get; set; }
        public string? ProductID { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
    }
}
