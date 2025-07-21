using E_BangDomain.ResponseDtos.SharedResponseDtos;

namespace E_BangDomain.ResponseDtos.Account
{
    public class ConfirmEmailResponseDto : SuccessResponseDto
    {
        protected override void UpdateMessage()
        {
            Message = IsSuccess
                ? "Email confirmed successfully."
                : "Email confirmation failed. Please try again.";
        }
    }
}
