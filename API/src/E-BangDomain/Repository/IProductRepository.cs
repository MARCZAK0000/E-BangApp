using E_BangDomain.EntetiesDto.Commands.Product;
using E_BangDomain.Entities;
using E_BangDomain.ModelDtos.Pagination;
using E_BangDomain.Pagination;

namespace E_BangDomain.Repository
{
    public interface IProductRepository
    {
        /// <summary>
        /// Create new shop product 
        /// </summary>
        /// <param name="createProductDto">Data transfer object</param>
        /// <param name="cancellationToken">Cancellation Token</param>
        /// <returns>True or False</returns>
        Task<bool> CreateProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken);
        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="updateProductDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>True of False</returns>
        Task<bool> UpdateProductAsync(Product product, CreateProductDto updateProductDto, CancellationToken cancellationToken);
        Task<bool> CreateProductPriceAndCountInformationsAsync(string productId, decimal price, int count, CancellationToken cancellationToken);
        /// <summary>
        /// Use to update count of product
        /// </summary>
        /// <param name="productInformations"></param>
        /// <param name="productCount"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        Task<bool> UpdateCount(ProductPriceCount count, int productCount);
        Task<bool> InsertProductFileAsync(Product product, InsertProductFileDto updateProductImageDto, CancellationToken cancellationToken);
        Task<Product?> GetProductByIdAsync(string productId, CancellationToken cancellationToken);
        Task<PaginationBase<Product>> GetAllProductsByShopId(PaginationModelDto paginationModelDto, string shopId, CancellationToken token);
        Task<Product?> GetProductAndInformationsByIdAsync(string productId, CancellationToken cancellationToken);
        Task<PaginationBase<Product>> GetAllProductAndInfromationsByShopIDAsync(PaginationModelDto paginationModelDto, string shopId, CancellationToken cancellationToken);
        Task<ProductInformations?> GetProductInformationsAsyncByID(string productId, CancellationToken cancellationToken);
        Task<ProductPriceCount?> GetProductPriceAndCountByIdAsync(string productId, CancellationToken cancellationToken);
    }
}
