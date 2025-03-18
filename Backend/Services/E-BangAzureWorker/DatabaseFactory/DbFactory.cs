using E_BangAzureWorker.DbRepository;

namespace E_BangAzureWorker.DatabaseFactory
{
    public class DbFactory : IDbFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DbFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IDbBase RoundRobin(bool IsRemove)
        {
            if (IsRemove)
            {
                return _serviceProvider.GetRequiredService<DbRemoveFIles>();
            }
            return _serviceProvider.GetRequiredService<DbAddFiles>();
        }
    }
}
