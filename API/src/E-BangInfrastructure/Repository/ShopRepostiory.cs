using E_BangDomain.Entities;
using E_BangDomain.MaybePattern;
using E_BangDomain.ModelDtos.Pagination;
using E_BangDomain.Pagination;
using E_BangDomain.Repository;
using E_BangInfrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace E_BangInfrastructure.Repository
{
    public class ShopRepostiory : IShopRepository
    {
        private readonly ProjectDbContext _dbContext;

        public ShopRepostiory(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateShopAsync(Shop shop, CancellationToken token)
        {
            await _dbContext.Shop.AddAsync(entity: shop, cancellationToken: token);
            return await _dbContext.SaveChangesAsync(token) > 0;
        }

        public async Task<bool> CreateShopBranchAsync(List<ShopBranchesInformations> shopBranchesInformations, CancellationToken token)
        {
            await _dbContext.ShopAddressInformations.AddRangeAsync(entities: shopBranchesInformations, token);
            return await _dbContext.SaveChangesAsync(token) > 0;
        }

        public async Task<Maybe<Shop>> GetShopByIDAsync(string shopId, CancellationToken cancellationToken)
        {
            return new Maybe<Shop>(await
                _dbContext
                .Shop
                .Where(pr => pr.ShopId == shopId)
                .FirstOrDefaultAsync(cancellationToken));
        }

        public async Task<bool> UpdateShopAsync(Shop shop, CancellationToken cancellationToken)
        {
            _dbContext.Shop.Update(shop);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }
        public async Task<bool> UpdateMainShopAsync(string shopID, string branchId, CancellationToken cancellationToken) => 
            await _dbContext
                .ShopAddressInformations
                .Where(pr => pr.ShopID == shopID && pr.ShopBranchId == branchId)
                .ExecuteUpdateAsync(pr =>
                    pr.SetProperty(p => p.IsMainShop, true)
                        .SetProperty(p => p.LastModifiedTime, DateTime.Now), cancellationToken: cancellationToken) > 0;
        public async Task<bool> RemoveShopAsync(string shopId, CancellationToken token)
        {
            SqlParameter param1 = new("@param1", shopId);
            await _dbContext
                .Database
                .ExecuteSqlRawAsync("EXEC Shop.sp_RemoveShop @param1", param1, token);
            return true;
        }

        public async Task<bool> RemoveShopBranchAsync(ShopBranchesInformations delete, CancellationToken token)
        {
           return await _dbContext
                .ShopAddressInformations
                .Where(pr=>pr.ShopID==delete.ShopID)
                .ExecuteDeleteAsync(token) > 0;
        }

        public async Task<PaginationBase<Shop>> GetAllShopsAsync(PaginationModelDto paginationModelDto, CancellationToken cancellationToken)
        {
            var paginationBuilder = new PaginationBuilder<Shop>();
            int totalCount = await _dbContext.Shop.CountAsync(cancellationToken);
            List<Shop> shops = await _dbContext
                .Shop
                .Skip((paginationModelDto.PageIndex - 1) * paginationModelDto.PageSize)
                .Take(paginationModelDto.PageSize)
                .ToListAsync(cancellationToken: cancellationToken);

            return paginationBuilder
                .SetItems(shops)
                .SetPageSize(paginationModelDto.PageSize)
                .SetPageIndex(paginationModelDto.PageIndex)
                .SetTotalItmesCount(totalCount)
                .SetPageCount(paginationModelDto.PageSize, paginationModelDto.PageIndex)
                .Build();

        }

        public async Task<List<ShopBranchesInformations>> GetShopBranchesByShopIdAsync(string shopBranchId, CancellationToken cancellationToken)
        {
            return await _dbContext
                .ShopAddressInformations
                .Where(pr => pr.ShopID == shopBranchId)
                .ToListAsync(cancellationToken: cancellationToken);
        }

        public async Task<List<string>> ListOfStaffIdInShop(string shopId, CancellationToken cancellationToken)
        {

            return await _dbContext
                .Staff
                .Where(pr => pr.ShopId == shopId)
                .Select(pr => pr.AccountId)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AddStaffToShop(List<ShopStaff> staff, CancellationToken cancellationToken)
        {
            await _dbContext.Staff.AddRangeAsync(staff, cancellationToken);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> UpdateShopBranchAsync(ShopBranchesInformations branch, CancellationToken cancellationToken)
        {
            _dbContext.ShopAddressInformations.Update(branch);
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> RemoveMainShopAsync(string shopId, string shopBranchId, CancellationToken cancellationToken)
        {
            return await _dbContext
                .ShopAddressInformations
                .Where(pr => pr.ShopID == shopId && pr.ShopBranchId == shopBranchId)
                .ExecuteUpdateAsync(pr =>
                    pr.SetProperty(p => p.IsMainShop, false)
                        .SetProperty(p => p.LastModifiedTime, DateTime.Now), cancellationToken: cancellationToken) > 0;
        }

        public async Task<Maybe<ShopBranchesInformations>> GetShopBranchByIdAsync(string shopBranchId, CancellationToken cancellationToken)
        {
            var response = await _dbContext
                .ShopAddressInformations
                .Where(pr => pr.ShopBranchId == shopBranchId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
            return new Maybe<ShopBranchesInformations>(response);
        }

        public async Task<bool> RemoveAllShopBranchesAsync(string shopId, CancellationToken cancellationToken)
        {
            return await _dbContext
                .ShopAddressInformations
                .Where(pr => pr.ShopID == shopId)
                .ExecuteDeleteAsync(cancellationToken: cancellationToken) > 0;
        }
    }
}