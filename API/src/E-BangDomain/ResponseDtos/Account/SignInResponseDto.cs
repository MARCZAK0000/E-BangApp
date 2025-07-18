using E_BangDomain.ResponseDtos.SharedResponseDtos;

namespace E_BangDomain.ResponseDtos.Account
{
    public class SignInResponseDto : SuccessResponseDto
    {
        protected override void UpdateMessage()
        {
            Message = IsSuccess
                ? "Sign-in successful."
                : "Sign-in failed. Please check your credentials and try again.";
        }
    }
}
