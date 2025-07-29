using E_BangDomain.ResponseDtos.SharedResponseDtos;

namespace E_BangDomain.ResponseDtos.Shop
{
    public class AddShopStaffResponseDto : SuccessResponseDto
    {
        protected override void UpdateMessage()
        {
            Message = IsSuccess?"Shop staff added successfully.":"Failed to add shop staff.";   
        }
    }
}
