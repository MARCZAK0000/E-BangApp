using E_BangAzureWorker.DbRepository;
using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.DatabaseFactory
{
    public interface IDbStrategy
    {
        Task<bool> StrategyRoundRobin(bool IsRemove, List<FileChangesInformations> fileChangesInformations, CancellationToken token);
    }
}
