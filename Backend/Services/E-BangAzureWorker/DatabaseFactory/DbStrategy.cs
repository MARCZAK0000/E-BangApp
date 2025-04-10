using E_BangAzureWorker.DbRepository;
using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.DatabaseFactory
{
    public class DbStrategy : IDbStrategy
    {
        private readonly Func<bool, IDbBase> _strategy;

        public DbStrategy(Func<bool, IDbBase> strategy)
        {
            _strategy = strategy;
        }

        public async Task<bool> StrategyRoundRobin(bool IsRemove, List<FileChangesInformations> fileChangesInformations, CancellationToken token)
        {
            var func = _strategy.Invoke(IsRemove);
            return await func.HandleAsync(fileChangesInformations, token);
        }
    }
}
