using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangDomain.ResponseDtos.Account
{
    public class TwoWayTokenResponseDto : SignInResponseDto
    {
        public string TwoWayToken { get; set; }
    }
}
