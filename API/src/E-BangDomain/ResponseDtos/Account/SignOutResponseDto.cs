using E_BangDomain.ResponseDtos.SharedResponseDtos;

namespace E_BangDomain.ResponseDtos.Account
{
    public class SignOutResponseDto : SuccessResponseDto
    {
        protected override void UpdateMessage()
        {
            Message = IsSuccess? "You have been successfully signed out." :
                "An error occurred while signing you out. Please try again later.";
        }
    }
}
