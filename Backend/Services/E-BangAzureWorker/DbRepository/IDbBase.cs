using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.DbRepository
{
    public interface IDbBase
    {
        Task<bool> HandleAsync(List<FileChangesInformations> fileChangesInformations, CancellationToken token);
    }
}
