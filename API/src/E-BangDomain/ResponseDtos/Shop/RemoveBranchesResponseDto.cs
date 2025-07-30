using E_BangDomain.ResponseDtos.SharedResponseDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangDomain.ResponseDtos.Shop
{
    public class RemoveBranchesResponseDto : SuccessResponseDto
    {
        protected override void UpdateMessage()
        {
            Message = IsSuccess ? "Branches removed successfully" : "Cannot remove branches";
        }
    }
}
