using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.AzureStrategy
{
    public interface IAzureStrategy
    {
        Task<FileChangesResponse> AzureBlobStrategy (MessageModel model, CancellationToken token);
    }
}
