using E_BangDomain.ResponseDtos.SharedResponseDtos;

namespace E_BangDomain.ResponseDtos.User
{
    public class UpdatePasswordResponseDto : SuccessResponseDto
    {
        protected override void UpdateMessage()
        {
             Message = IsSuccess?"Password updated successfully." :
                "Failed to update password. Please try again.";
        }
    }
}
