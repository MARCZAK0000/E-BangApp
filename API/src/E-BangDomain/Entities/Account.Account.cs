using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangDomain.Entities
{
    [Table("Account", Schema = "Account")]
    public partial class Account : IdentityUser
    {

    }
    public partial class Account
    {
        public Users User { get; set; }
    }
}
