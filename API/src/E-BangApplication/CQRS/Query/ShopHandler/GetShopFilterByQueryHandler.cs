using AutoMapper;
using E_BangDomain.Entities;
using E_BangDomain.EntitiesDto.Shop;
using E_BangDomain.ModelDtos.Pagination;
using E_BangDomain.Pagination;
using E_BangDomain.Repository;
using E_BangDomain.ResponseDtos.Shop;
using MyCustomMediator.Interfaces;

namespace E_BangApplication.CQRS.Query.ShopHandler
{
    public class GetShopFilterByQueryHandler : IRequestHandler<GetShopByFilterQuery, GetShopByFilterResponseDto>
    {
        private readonly IShopRepository _shopRepository;
        private readonly IMapper _mapper;

        public GetShopFilterByQueryHandler(IShopRepository shopRepository, IMapper mapper)
        {
            _shopRepository = shopRepository;
            _mapper = mapper;
        }

        public async Task<GetShopByFilterResponseDto> Handle(GetShopByFilterQuery request, CancellationToken token)
        {
            var response = new GetShopByFilterResponseDto();

            PaginationBase<Shop> shops = 
                await _shopRepository.GetAllShopsAsync(request.PaginationModelDto, token, request.FilterName, request.ShopTypeId);

            if (shops.Items.Count == 0)
            {
                return response;
            }

            response.Data = new PaginationBase<ShopDto>
            {
                Items = _mapper.Map<List<ShopDto>>(shops.Items),
                TotalItemsCount = shops.TotalItemsCount,
                PageSize = shops.PageSize,
                PageIndex = shops.PageIndex,
                PageCount = shops.PageCount
            };
            response.IsSuccess = true;
            return response;
        }
    }

    public class GetShopByFilterQuery : IRequest<GetShopByFilterResponseDto>
    {
        public string? FilterName { get; set; }
        public int ShopTypeId { get; set; } = 0;
        public PaginationModelDto PaginationModelDto { get; set; }
    }
    
}
