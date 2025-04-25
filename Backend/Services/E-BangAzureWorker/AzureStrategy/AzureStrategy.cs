using E_BangAppRabbitSharedClass.AzureRabbitModel;
using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.AzureStrategy
{

    public class AzureStrategy : IAzureStrategy
    {
        private readonly Func<AzureStrategyEnum, IAzureBase> _strategy;
        public AzureStrategy(Func<AzureStrategyEnum, IAzureBase> strategy)
        {
            _strategy = strategy;
        }
        public async Task<FileChangesResponse> AzureBlobStrategy(AzureMessageModel model, CancellationToken token)
        {
            var handler = _strategy.Invoke(model.AzureStrategyEnum);
            return await handler.HandleAzureAsync(model, token);
        }
    }
}
