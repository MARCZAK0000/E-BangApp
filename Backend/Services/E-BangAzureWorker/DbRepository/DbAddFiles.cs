using E_BangAzureWorker.Model;

namespace E_BangAzureWorker.DbRepository
{
    public class DbAddFiles : IDbBase
    {
        public Task<bool> HandleAsync(FileChangesInformations fileChangesInformations, CancellationToken token)
        {
            throw new NotImplementedException();
        }
    }
}
