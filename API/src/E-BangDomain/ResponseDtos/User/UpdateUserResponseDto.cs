using E_BangDomain.ResponseDtos.SharedResponseDtos;

namespace E_BangDomain.ResponseDtos.User
{
    public class UpdateUserResponseDto : SuccessResponseDto
    {
        protected override void UpdateMessage()
        {
            Message = IsSuccess? "User updated successfully." : "User not updated";
        }
    }
}
