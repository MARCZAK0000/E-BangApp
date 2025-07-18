using E_BangDomain.ResponseDtos.SharedResponseDtos;

namespace E_BangDomain.ResponseDtos.Account
{
    public class TwoWayTokenResponseDto : SuccessResponseDto
    {
        public string TwoWayToken { get; set; } = string.Empty;

        public bool IsTokenGenerated { get; set; } = false;

        protected override void UpdateMessage()
        {
            Message = IsSuccess && IsTokenGenerated
                ? "Two-way token generated successfully."
                    : IsSuccess && !IsTokenGenerated ?
                        "Token generation is not required. Verifiation completed" :
                            "Veryfication failed. Please try again.";
        }
    }
}
