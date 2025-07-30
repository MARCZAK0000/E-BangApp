using E_BangDomain.ResponseDtos.SharedResponseDtos;

namespace E_BangDomain.ResponseDtos.Shop
{
    public class UpdateShopResponseDto : SuccessResponseDto
    {
        protected override void UpdateMessage()
        {
            Message = IsSuccess ? "Shop updated successfully" : "Cannot update shop";
        }
    }
}
