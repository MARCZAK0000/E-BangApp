using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangDomain.RequestDtos.AccountRepositoryDtos
{
    public  class ResetPasswordDto : CredentialsAccountDto
    {
        public string ConfirmPassword { get; set; } 
    }
}
