
namespace E_BangAzureWorker.Model
{
    public class BlobContainer
    {
        public int Id { get ; set ; }
        public string Name { get ; set ; }
        public string Description { get ; set ; }
        public string RootFilePath { get ; set ; }
        public bool Enabled { get ; set ; } = false;
        public DateTime LastUpdateTime { get ; set ; }
        public int BlobRootPathID {  get ; set ; }
        public BlobContainerRoot BlobContainerRoot { get ; set ; }
    }
}
