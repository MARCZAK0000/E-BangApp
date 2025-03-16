using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.AzureFactory
{
    public interface IAzureBase
    {
        Task<bool> HandleAzureAsync(MessageModel model, CancellationToken token);
    }
}
