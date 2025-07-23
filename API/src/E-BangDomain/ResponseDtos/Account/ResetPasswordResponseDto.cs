using E_BangDomain.ResponseDtos.SharedResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangDomain.ResponseDtos.Account
{
    public class ResetPasswordResponseDto : SuccessResponseDto
    {
        protected override void UpdateMessage()
        {
            Message = IsSuccess? "Password has been updated successfully": ""
        }
    }
}
