using E_BangAzureWorker.AzureBaseRepo;

namespace E_BangAzureWorker.AzureFactory
{
    public class AzureFactory : IAzureFactory
    {
        public IAzureBase RoundRobin(AzureStrategyEnum azure)
        {
            return azure switch
            {
                AzureStrategyEnum.Add => new AzureAddFileRepository(),
                AzureStrategyEnum.Remove => new AzureRemoveFileRepository(),
                _ => throw new NotSupportedException(),
            };
        } 
    }
}
