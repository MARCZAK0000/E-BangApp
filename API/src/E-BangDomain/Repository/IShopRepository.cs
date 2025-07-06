using E_BangDomain.Entities;
using E_BangDomain.EntitiesDto.Commands.Shop;
using E_BangDomain.ModelDtos.Pagination;
using E_BangDomain.Pagination;

namespace E_BangDomain.Repository
{
    public interface IShopRepository
    {
        Task<Shop?> GetShopByIDAsync(string shopId, CancellationToken cancellationToken);
        Task<PaginationBase<Shop>> GetAllShopsAsync(PaginationModelDto paginationModelDto, CancellationToken cancellationToken);
        Task<bool> CreateShopAsync(CreateShopDto createShopDto, CancellationToken token);
        Task<bool> CreateShopBranchAsync(string shopID, CreateShopBranchDto createShopBranchDto, CancellationToken token);
        Task<bool> UpdateShopAsync(Shop shop, CreateShopDto createShopDto, CancellationToken cancellationToken);
        Task<bool> UpdateShopBranchAsync(ShopBranchesInformations branch, CreateShopBranchDto create, CancellationToken cancellationToken);
        Task<bool> ValidMainShopAsync(string shopID, CancellationToken cancellationToken);
        Task<bool> RemoveShopAsync(string shopId, CancellationToken token);
        Task<bool> RemoveShopBranchAsync(string shopId, string shopBranchId, CancellationToken token);
    }
}
