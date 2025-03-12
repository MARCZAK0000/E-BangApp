
namespace E_BangAzureWorker.Model
{
    public class ContainerSettings : IContainerSettings
    {
        public string RootPath { get ; set ; }
        public List<Container> Containers { get ; set ; }
    }
}
