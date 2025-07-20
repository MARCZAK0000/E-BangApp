using E_BangAppRabbitSharedClass.AzureRabbitModel;
using E_BangDomain.Entities;
using E_BangDomain.EntitiesDto.Commands.Product;
using E_BangDomain.Extensions;
using E_BangDomain.HelperRepository;
using E_BangDomain.ModelDtos.MessageSender;
using E_BangDomain.ModelDtos.Pagination;
using E_BangDomain.Pagination;
using E_BangDomain.Repository;
using E_BangInfrastructure.Database;
using E_BangInfrastructure.HelperRepository;
using Microsoft.EntityFrameworkCore;

namespace E_BangInfrastructure.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProjectDbContext _projectDbContext;

        private readonly IRabbitSenderRepository _rabbitSenderRepository;

        public ProductRepository(ProjectDbContext projectDbContext, IRabbitSenderRepository rabbitSenderRepository)
        {
            _projectDbContext = projectDbContext;
            _rabbitSenderRepository = rabbitSenderRepository;
        }

        #region Commands
        public async Task<bool> CreateProductAsync(CreateProductDto createProductDto, CancellationToken cancellationToken)
        {
            await _projectDbContext.Products.AddAsync(entity: new Product
            {
                ProductName = createProductDto.ProductName,
                ProductDescription = createProductDto.ProductDescription,
                ShopID = createProductDto.ShopId,
            }, cancellationToken: cancellationToken);

            return true;
        }
        public async Task<bool> CreateProductPriceAndCountInformationsAsync(string productId, decimal price, int count, CancellationToken cancellationToken)
        {
            await _projectDbContext
                .ProductPrice
                .AddAsync(new ProductPrice
                {
                    Price = price,
                    ProductID = productId,
                    ProductCount = count
                }, cancellationToken);
            return true;
        }
        public Task<bool> UpdateCount(ProductPrice productPrice, int productCount)
        {
            productPrice.ProductCount = productCount;
            return Task.FromResult(true);
        }
        public Task<bool> UpdateProductAsync(Product product, CreateProductDto updateProductDto, CancellationToken cancellationToken)
        {
            product.ProductName = updateProductDto.ProductName;
            product.ProductDescription = updateProductDto.ProductDescription;
            product.LastModifiedTime = DateTime.UtcNow;
            return Task.FromResult(true);
        }

        public async Task<bool> InsertProductFileAsync(Product product, InsertProductFileDto updateProductImageDto, CancellationToken cancellationToken)
        {
            product.ProductInformations ??= [];
            AzureMessageModel messageModel = new AzureMessageModel()
            {
                ProductID = product.ProductId,
                AzureStrategyEnum = AzureStrategyEnum.Add,
                ContainerID = 2,
                Data = []
            };
            AzureMessageData messageData = new()
            {
                Data = await updateProductImageDto.File.GetBytesAsync(),
                DataName = updateProductImageDto.File.FileName,
                DataType = Path.GetExtension(updateProductImageDto.File.FileName)
            };
            messageModel.Data.Add(messageData);

            await _rabbitSenderRepository.AddMessageToQueue(new RabbitMessageBaseDto<AzureMessageModel>
            {
                Message = messageModel,
                RabbitChannel = E_BangDomain.Enums.ERabbitChannel.AzureChannel
            }, cancellationToken);

            return true;
        }
        #endregion

        #region Handle
        public async Task<Product?> GetProductByIdAsync(string productId, CancellationToken cancellationToken)
            => await _projectDbContext.Products.Where(pr => pr.ProductId == productId).FirstOrDefaultAsync(cancellationToken);

        public async Task<PaginationBase<Product>> GetAllProductsByShopId(PaginationModelDto paginationModelDto, string shopId, CancellationToken token)
        {
            PaginationBuilder<Product> builder = new();

            int TotalCount = await _projectDbContext.Products.CountAsync(token);
            List<Product> products = await _projectDbContext
                .Products
                .Where(pr => pr.ShopID == shopId)
                .Skip((paginationModelDto.PageIndex - 1) * paginationModelDto.PageSize)
                .Take(paginationModelDto.PageSize)
                .ToListAsync(token);

            return builder
                .SetItems(products)
                .SetTotalItmesCount(TotalCount)
                .SetPageIndex(paginationModelDto.PageIndex)
                .SetPageSize(paginationModelDto.PageSize)
                .SetPageCount(paginationModelDto.PageSize, TotalCount)
                .Build();
        }

        public async Task<Product?> GetProductAndInformationsByIdAsync(string productId, CancellationToken cancellationToken)
            => await _projectDbContext.Products
                .Where(pr => pr.ProductId == productId)
                .Include(pr => pr.ProductInformations)
                .Include(pr => pr.ProductCountPrice)
                .FirstOrDefaultAsync(cancellationToken);

        public async Task<PaginationBase<Product>> GetAllProductAndInfromationsByShopIDAsync(PaginationModelDto paginationModelDto, string shopId, CancellationToken cancellationToken)
        {
            PaginationBuilder<Product> builder = new();

            int TotalCount = await _projectDbContext.Products.CountAsync(cancellationToken);
            List<Product> products = await _projectDbContext
                .Products
                .Where(pr => pr.ShopID == shopId)
                .Include(pr => pr.ProductInformations)
                .Include(pr => pr.ProductCountPrice)
                .Skip((paginationModelDto.PageIndex - 1) * paginationModelDto.PageSize)
                .Take(paginationModelDto.PageSize)
                .ToListAsync(cancellationToken);

            return builder
                .SetItems(products)
                .SetTotalItmesCount(TotalCount)
                .SetPageIndex(paginationModelDto.PageIndex)
                .SetPageSize(paginationModelDto.PageSize)
                .SetPageCount(paginationModelDto.PageSize, TotalCount)
                .Build();
        }

        public async Task<ProductInformations?> GetProductInformationsAsyncByID(string productId, CancellationToken cancellationToken)
            => await _projectDbContext.ProductInformations.Where(pr => pr.ProductID == productId).FirstOrDefaultAsync(cancellationToken);

        public async Task<ProductPrice?> GetProductPriceAndCountByIdAsync(string productId, CancellationToken cancellationToken)
            => await _projectDbContext.ProductPrice.Where(pr => pr.ProductID == productId).FirstOrDefaultAsync(cancellationToken);

        #endregion
    }
}
