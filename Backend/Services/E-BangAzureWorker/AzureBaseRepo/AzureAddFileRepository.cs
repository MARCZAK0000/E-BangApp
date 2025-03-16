using E_BangAzureWorker.AzureFactory;
using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.AzureBaseRepo
{
    public class AzureAddFileRepository : IAzureBase
    {
        public Task<bool> HandleAzureAsync(MessageModel model, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
