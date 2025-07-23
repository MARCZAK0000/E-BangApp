using E_BangDomain.ResponseDtos.SharedResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangDomain.ResponseDtos.Account
{
    public class RefreshTokenResponseDto : SuccessResponseDto
    {
        protected override void UpdateMessage()
        {
            Message = IsSuccess ? "Token has been refreshed":"Token has not been refreshed successfully";
        }
    }
}
