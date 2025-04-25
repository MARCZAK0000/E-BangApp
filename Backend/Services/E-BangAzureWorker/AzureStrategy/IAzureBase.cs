using E_BangAppRabbitSharedClass.AzureRabbitModel;
using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.AzureStrategy
{
    public interface IAzureBase
    {
        Task<FileChangesResponse> HandleAzureAsync(AzureMessageModel model, CancellationToken token);
    }
}
