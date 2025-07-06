using E_BangDomain.EntetiesDto.Commands.Shop;
using E_BangDomain.Entities;
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

        public async Task<bool> CreateShopAsync(CreateShopDto createShopDto, CancellationToken token)
        {
            await _dbContext.Shop.AddAsync(entity: new Shop()
            {
                ShopName = createShopDto.ShopName,
                ShopTypeId = createShopDto.ShopTypeID,
                ShopDescription = createShopDto.ShopDescription,
            }, cancellationToken: token);
            return true;
        }

        public async Task<bool> CreateShopBranchAsync(string shopID, CreateShopBranchDto createShopBranchDto, CancellationToken token)
        {
            await _dbContext
                .ShopAddressInformations
                .AddAsync(entity: new ShopBranchesInformations
                {
                    ShopID = shopID,
                    ShopCity = createShopBranchDto.ShopCity,
                    ShopCountry = createShopBranchDto.ShopCountry,
                    ShopPostalCode = createShopBranchDto.ShopPostalCode,
                    ShopStreetName = createShopBranchDto.ShopStreetName,
                    IsMainShop = createShopBranchDto.IsMainShop,
                }, cancellationToken: token);
            return true;
        }

        public async Task<Shop?> GetShopByIDAsync(string shopId, CancellationToken cancellationToken)
            => await _dbContext.Shop.Where(pr => pr.ShopId == shopId).FirstOrDefaultAsync(cancellationToken);

        public async Task<bool> UpdateShopAsync(Shop shop, CreateShopDto createShopDto, CancellationToken cancellationToken)
        {
            shop.ShopName = createShopDto.ShopName;
            shop.ShopDescription = createShopDto.ShopDescription;
            shop.ShopTypeId = createShopDto.ShopTypeID;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> UpdateShopBranchAsync(ShopBranchesInformations branch, CreateShopBranchDto create, CancellationToken cancellationToken)
        {
            branch.ShopCity = create.ShopCity;
            branch.ShopCountry = create.ShopCountry;
            branch.ShopPostalCode = create.ShopPostalCode;
            branch.ShopStreetName = create.ShopStreetName;
            branch.IsMainShop = create.IsMainShop;

            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<bool> ValidMainShopAsync(string shopID, CancellationToken cancellationToken) => await _dbContext
                .ShopAddressInformations
                .Where(pr => pr.ShopID == shopID && pr.IsMainShop)
                .AnyAsync(cancellationToken);
        public async Task<bool> RemoveShopAsync(string shopId, CancellationToken token)
        {
            SqlParameter param1 = new("@param1", shopId);
            await _dbContext
                .Database
                .ExecuteSqlRawAsync("EXEC Shop.sp_RemoveShop @param1", param1, token);
            return true;
        }

        public async Task<bool> RemoveShopBranchAsync(string shopId, string shopBranchId, CancellationToken token)
        {
            SqlParameter param1 = new("@param1", shopId);
            SqlParameter param2 = new("@param2", shopBranchId);
            await _dbContext
                .Database
                .ExecuteSqlRawAsync("EXEC Shop.sp_RemoveShopBranch @param1, @param2", [param1, param2], cancellationToken: token);
            return true;
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
    }
}
