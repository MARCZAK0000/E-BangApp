using E_BangAzureWorker.Database;
using E_BangAzureWorker.Model;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace E_BangAzureWorker.DbRepository
{
    public class DbAddFiles : IDbBase
    {
        private readonly ServiceDbContext _dbContext;

        private readonly ILogger<DbAddFiles> _logger;
        public DbAddFiles(ServiceDbContext dbContext, ILogger<DbAddFiles> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> HandleAsync(List<FileChangesInformations> fileChangesInformations, CancellationToken token)
        {
            var blobItems = fileChangesInformations.Select(x => new BlobItems
            {
                AccountId = x.AccountID,
                BlobItemName = x.FileName,
                BlobItemType = x.FileType,
                ContainerID = x.ContainerId,
                ProductId = x.ProductID,
                LastUpdateTime = DateTime.Now,
            })
            .ToList();

            using IDbContextTransaction dbTransaction = await _dbContext.Database.BeginTransactionAsync(token);
            try
            {
                await _dbContext.Items.AddRangeAsync(blobItems, token);
                await dbTransaction.CommitAsync(token);
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Rollback transaction when adding files at {DateTime}", DateTime.Now)
                await dbTransaction.RollbackAsync(token);
                throw;
            }
        }
    }
}
