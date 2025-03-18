using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.AzureFactory
{
    public interface IAzureBase
    {
        Task<FileChangesResponse> HandleAzureAsync(MessageModel model, CancellationToken token);
    }
}
