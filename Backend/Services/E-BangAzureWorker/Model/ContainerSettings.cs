
namespace E_BangAzureWorker.Model
{
    
    public class ContainerSettings
    {
        public string RootPath { get ; set ; }
        public List<BlobContainer> Containers { get ; set ; }
    }
}
