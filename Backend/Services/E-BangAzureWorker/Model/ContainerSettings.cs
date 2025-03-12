
namespace E_BangAzureWorker.Model
{
    public class ContainerSettings : IContainerSettings
    {
        public string RootPath { get ; set ; }
        public List<BlobContainer> Containers { get ; set ; }
    }
}
