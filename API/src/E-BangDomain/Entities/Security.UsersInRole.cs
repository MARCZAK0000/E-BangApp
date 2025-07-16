using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangDomain.Entities
{
    [Table("UsersInRole", Schema = "Security")]
    public partial class UsersInRole
    {
        [Key]
        public string UserInRoleID { get; set; } = Guid.NewGuid().ToString();
        public string UserID { get; set; } 
        public string RoleID { get; set; }
        public DateTime LastUpdateTime = DateTime.Now;
    }

    public partial class UsersInRole
    {
        public Users Users { get; set; }
        public Roles Roles { get; set; }    
    }
}
