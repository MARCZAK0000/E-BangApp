
namespace E_BangAzureWorker.Model
{
    public interface IContainerSettings
    {
        string RootPath { get; set; }
        List<BlobContainer> Containers { get; set; }
    }
    public class ContainerSettings : IContainerSettings
    {
        public string RootPath { get ; set ; }
        public List<BlobContainer> Containers { get ; set ; }
    }
}
