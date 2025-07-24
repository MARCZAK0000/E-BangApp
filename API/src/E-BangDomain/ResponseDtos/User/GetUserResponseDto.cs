using E_BangDomain.EntitiesDto.User;
using E_BangDomain.ResponseDtos.SharedResponseDtos;

namespace E_BangDomain.ResponseDtos.User
{
    public class GetUserResponseDto<T> : SuccessResponseDto<T> where T : class
    {
        protected override void UpdateMessage()
        {
            Message = IsSuccess ? "Data send " : "Data cannot be send";
        }
    }
}
