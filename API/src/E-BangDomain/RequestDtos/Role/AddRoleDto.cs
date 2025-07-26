using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangDomain.RequestDtos.Role
{
    public class AddRoleDto
    {
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public int RoleLevel { get; set; }
    }
}
