using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangDomain.Entities
{
    [Table(name:"Role", Schema = "Security")]
    public partial class Roles
    {
        [Key]
        public string RoleID { get; set; } = Guid.NewGuid().ToString();
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public DateTime LastUpdate { get; set; } = DateTime.Now;   
    }
    public partial class Roles
    {
        public List<Users> Users { get; set; }
        public List<ActionInRole> ActionsInRole { get; set; }

        public List<UsersInRole> UsersInRoles { get; set; }
    }
}
