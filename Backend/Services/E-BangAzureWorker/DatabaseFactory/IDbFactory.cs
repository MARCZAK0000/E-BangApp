using E_BangAzureWorker.DbRepository;

namespace E_BangAzureWorker.DatabaseFactory
{
    public interface IDbFactory
    {
        IDbBase RoundRobin(bool IsRemove);
    }
}
