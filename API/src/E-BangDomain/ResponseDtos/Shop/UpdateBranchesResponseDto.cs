using E_BangDomain.ResponseDtos.SharedResponseDtos;

namespace E_BangDomain.ResponseDtos.Shop
{
    public class UpdateBranchesResponseDto : SuccessResponseDto
    {
        protected override void UpdateMessage()
        {
            Message = IsSuccess ? "Branches updated successfully" : "Cannot update branches";
        }
    }
}
