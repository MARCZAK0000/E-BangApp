using E_BangAppRabbitSharedClass.AzureRabbitModel;
using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.AzureStrategy
{
    public interface IAzureStrategy
    {
        Task<FileChangesResponse> AzureBlobStrategy (AzureMessageModel model, CancellationToken token);
    }
}
