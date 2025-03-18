using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.DbRepository
{
    public interface IDbBase
    {
        Task<bool> HandleAsync(FileChangesInformations fileChangesInformations, CancellationToken token);
    }
}
