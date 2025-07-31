using E_BangDomain.EntitiesDto.Shop;
using E_BangDomain.Pagination;
using E_BangDomain.ResponseDtos.SharedResponseDtos;

namespace E_BangDomain.ResponseDtos.Shop
{
    public class GetShopByFilterResponseDto : SuccessResponsePaginationDto<PaginationBase<ShopDto>>
    {
        protected override void UpdateMessage()
        {
            Message = IsSuccess
                ? "Shops retrieved successfully."
                : "Failed to retrieve shops.";
        }
    }
}
