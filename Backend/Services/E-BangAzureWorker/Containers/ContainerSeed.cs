using E_BangAzureWorker.Database;
using E_BangAzureWorker.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace E_BangAzureWorker.Containers
{
    public class ContainerSeed
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly ILogger<ContainerSeed> _logger;

        private readonly IContainerSettings _containerSettings;
        public ContainerSeed(IServiceScopeFactory serviceScopeFactory, ILogger<ContainerSeed> logger, IContainerSettings containerSettings)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _containerSettings = containerSettings;
            _logger.LogInformation("{Container} initialized at {DateTime}", nameof(ContainerSeed), DateTime.Now);
        }

        public async Task InvokeSeed()
        {
            _logger.LogInformation("Invoke Seeding in {Container} at {DateTime}", nameof(ContainerSeed), DateTime.Now);
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _serviceDbContext = scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
                if (!await _serviceDbContext.Database.CanConnectAsync())
                {
                    throw new Exception("Cannot connect to Database");
                }
                if(await _serviceDbContext.Containers.AnyAsync())
                {
                    _logger.LogInformation("Db was Initalized");
                    return;
                }
                if(_containerSettings == null || _containerSettings.Containers.Count == 0)
                {
                    throw new Exception("Problem with connection settings");
                }
                using IDbContextTransaction transaction = await _serviceDbContext.Database.BeginTransactionAsync();
                try
                {

                }
                catch (Exception)
                {

                    throw;
                }
            }
            catch (Exception err)
            {
                _logger.LogCritical("Cannot connect to Database in {0}", nameof(ContainerSeed));
                throw;
            }
            
            
           
        }

    }
}
