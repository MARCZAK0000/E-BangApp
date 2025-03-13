using E_BangAzureWorker.Comaperer;
using E_BangAzureWorker.Database;
using E_BangAzureWorker.Model;
using Microsoft.EntityFrameworkCore;

namespace E_BangAzureWorker.Containers
{
    public class ContainerConnections
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly IContainerSettings _containerSettings;

        private readonly ILogger<ContainerConnections> _logger;

        public ContainerConnections(IServiceScopeFactory serviceScopeFactory, IContainerSettings containerSettings, ILogger<ContainerConnections> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _containerSettings = containerSettings;
            _logger = logger;
        }
        public async Task ValidateSettingsAsync()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            ServiceDbContext serviceDbContext = scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
            try
            {
                var rootPath = await serviceDbContext.ContainerRoot.FirstOrDefaultAsync();
                if (rootPath?.RootPath == null || !rootPath.RootPath.Equals(_containerSettings.RootPath, StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new Exception("Root Path is empty");
                }
                _logger.LogInformation("Root Path is correct : {DateTime}", DateTime.Now);

                var configurationSettings = await serviceDbContext.Containers.ToListAsync();
                var result = configurationSettings.Intersect(_containerSettings.Containers, new BlobContainerComparer()).ToList();

                if (result.Count != configurationSettings.Count)
                {
                    var differences = configurationSettings.Except(_containerSettings.Containers, new BlobContainerComparer()).Select(item => item.Name).ToList();
                    throw new Exception($"Invalid Data in Containers: {string.Join(Environment.NewLine, differences)}");
                }
            }
            catch (Exception err)
            {
                _logger.LogError("Error: {err} at {DateTime}", err, DateTime.Now);
                throw;
            }
        }
    }
}
