using E_BangDomain.Entities;
using E_BangDomain.MaybePattern;
using E_BangDomain.ModelDtos.Pagination;
using E_BangDomain.Pagination;

namespace E_BangDomain.Repository
{
    public interface IShopRepository
    {
        Task<Maybe<Shop>> GetShopByIDAsync(string shopId, CancellationToken cancellationToken);
        Task<PaginationBase<Shop>> GetAllShopsAsync(PaginationModelDto paginationModelDto, CancellationToken cancellationToken, string? filterName = null, int? typeId = null);
        Task<bool> CreateShopAsync(Shop createShopDto, CancellationToken token);
        Task<bool> CreateShopBranchAsync(List<ShopBranchesInformations> shopBranchesInformations, CancellationToken token);
        Task<bool> AddStaffToShop(List<ShopStaff> staff, CancellationToken cancellationToken);
        Task<bool> UpdateShopAsync(Shop shop, CancellationToken cancellationToken);
        Task<bool> UpdateShopBranchAsync(ShopBranchesInformations branch, CancellationToken cancellationToken);
        Task<bool> UpdateMainShopAsync(string shopID, string branchId, CancellationToken cancellationToken);
        Task<bool> RemoveShopAsync(string shopId, CancellationToken token);
        Task<bool> RemoveShopBranchAsync(ShopBranchesInformations delete, CancellationToken token);
        Task<bool> RemoveMainShopAsync(string shopId, string shopBranchId, CancellationToken cancellationToken);
        Task<List<string>> ListOfStaffIdInShop(string shopId, CancellationToken cancellationToken);
        Task<List<ShopBranchesInformations>> GetShopBranchesByShopIdAsync(string shopBranchId, CancellationToken cancellationToken);
        Task<Maybe<ShopBranchesInformations>> GetShopBranchByIdAsync(string shopBranchId, CancellationToken cancellationToken);
        Task<bool> RemoveAllShopBranchesAsync(string shopId, CancellationToken cancellationToken);
    }
}
