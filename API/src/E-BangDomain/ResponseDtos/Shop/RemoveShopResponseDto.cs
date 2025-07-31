using E_BangDomain.ResponseDtos.SharedResponseDtos;

namespace E_BangDomain.ResponseDtos.Shop
{
    public class RemoveShopResponseDto : SuccessResponseDto
    {
        protected override void UpdateMessage()
        {
            Message = IsSuccess
                ? "Shop removed successfully."
                : "Failed to remove shop.";
        }
    }
}
