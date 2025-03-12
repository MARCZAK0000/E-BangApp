
namespace E_BangAzureWorker.Model
{
    public class Container
    {
        public int Id { get ; set ; }
        public string Name { get ; set ; }
        public string Description { get ; set ; }
        public string RootFilePath { get ; set ; }
        public bool Enabled { get ; set ; } = false;
        public DateTime LastUpdateTime { get ; set ; }
    }
}
