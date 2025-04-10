using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.AzureStrategy
{
    public interface IAzureBase
    {
        Task<FileChangesResponse> HandleAzureAsync(MessageModel model, CancellationToken token);
    }
}
