using E_BangAzureWorker.AzureBaseRepo;

namespace E_BangAzureWorker.AzureFactory
{
    public class AzureFactory : IAzureFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public AzureFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAzureBase RoundRobin(AzureStrategyEnum azure)
        {
            return azure switch
            {
                AzureStrategyEnum.Add => _serviceProvider.GetRequiredService<AzureAddFileRepository>(),
                AzureStrategyEnum.Remove => _serviceProvider.GetRequiredService<AzureRemoveFileRepository>(),
                _ => throw new NotSupportedException(),
            };
        } 
    }
}
