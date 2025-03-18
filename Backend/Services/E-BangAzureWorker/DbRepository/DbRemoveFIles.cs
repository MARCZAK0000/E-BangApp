using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.DbRepository
{
    internal class DbRemoveFIles : IDbBase
    {
        public Task<bool> HandleAsync(FileChangesInformations fileChangesInformations, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
