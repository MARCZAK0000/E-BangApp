using E_BangDomain.ResponseDtos.SharedResponseDtos;

namespace E_BangDomain.ResponseDtos.Account
{
    public class RegisterAccountResponseDto : SuccessResponseDto
    {
        public string Email { get; set; } = string.Empty;
        protected override void UpdateMessage()
        {
            Message = IsSuccess
                ? "Account registered successfully."
                : "Account registration failed. Please try again.";
        }
    }
}
