using E_BangAzureWorker.Database;
using E_BangAzureWorker.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;

namespace E_BangAzureWorker.Containers
{
    public class ContainerSeed
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<ContainerSeed> _logger;

        private readonly ContainerSettings _containerSettings;
        public ContainerSeed(IServiceScopeFactory serviceScopeFactory, ILogger<ContainerSeed> logger, ContainerSettings containerSettings)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _containerSettings = containerSettings;
            _logger.LogInformation("{Container} initialized at {DateTime}", nameof(ContainerSeed), DateTime.Now);
        }
        public async Task MirageAsync()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var serviceDbContext = scope.ServiceProvider.GetRequiredService<ServiceDbContext>();
                if(await serviceDbContext.Database.CanConnectAsync())
                {
                    //if(!await serviceDbContext.Database.EnsureCreatedAsync())
                    //{
                    //    //return;
                    //};
                    var pendingMigrations = await serviceDbContext.Database.GetPendingMigrationsAsync();
                    if(pendingMigrations.Any())
                    {
                        await serviceDbContext.Database.MigrateAsync();
                        _logger.LogInformation("Migration at {Date}", DateTime.Now);
                    }
                }

            }
            catch (Exception)
            {
                _logger.LogError("Problem with Migrations: {Date}", DateTime.Now);
                throw;
            }
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
                if(await _serviceDbContext.Containers.AnyAsync() || await _serviceDbContext.ContainerRoot.AnyAsync())
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
                    await _serviceDbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.ContainerRoot ON");
                    var blobRoot = new BlobContainerRoot
                    {
                        Id = 1,
                        RootPath = _containerSettings.RootPath,
                        LastUpdateTime = DateTime.Now,
                    };

                    await _serviceDbContext.ContainerRoot.AddAsync(blobRoot);
                    await _serviceDbContext.SaveChangesAsync();
                    await _serviceDbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.ContainerRoot OFF");

                    await _serviceDbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Containers ON");
                    await _serviceDbContext.Containers.AddRangeAsync(_containerSettings.Containers.Select(pr=> new BlobContainer
                    {
                        Id = pr.Id,
                        Name = pr.Name,
                        Description = pr.Description,
                        Enabled = pr.Enabled,
                        LastUpdateTime = DateTime.Now,
                        RootFilePath = pr.RootFilePath,
                        BlobRootPathID = blobRoot.Id
                    }));
                    await _serviceDbContext.SaveChangesAsync();
                    await _serviceDbContext.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT dbo.Containers OFF");
                    await transaction.CommitAsync();    
                }
                catch (Exception err)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError("Error with database at {DateTime}: {err}", DateTime.Now, err);
                    throw;
                }
            }
            catch (Exception err)
            {
                _logger.LogCritical("Error in {container} at {DateTime}: {err}", nameof(ContainerSeed), DateTime.Now, err);
                throw;
            }
        }

    }
}
