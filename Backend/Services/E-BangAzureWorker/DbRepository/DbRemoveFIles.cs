using E_BangAzureWorker.Comaperer;
using E_BangAzureWorker.Database;
using E_BangAzureWorker.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace E_BangAzureWorker.DbRepository
{
    internal class DbRemoveFIles : IDbBase
    {
        private readonly ServiceDbContext _dbContext;

        private readonly ILogger<DbRemoveFIles> _logger;
        public DbRemoveFIles(ServiceDbContext dbContext, ILogger<DbRemoveFIles> logger)
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
            var findDeleteFiles = await _dbContext.Items
                .Intersect(blobItems, new BlobItemsComaparer())
                .ToListAsync(token);
            using IDbContextTransaction dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(token);
            try
            {
                _dbContext.Items.RemoveRange(findDeleteFiles);
                await dbContextTransaction.CommitAsync(token);
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Rollback transaction when removing files at {DateTime}", DateTime.Now);
                await dbContextTransaction.RollbackAsync(token);
                throw;
            }
        }
    }
}
