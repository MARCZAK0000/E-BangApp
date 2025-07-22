using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangDomain.Entities
{
    [Table("ActionInRole", Schema = "Security")]
    public partial class ActionInRole
    {
        [Key]
        public string ActionInRoleID { get; set; } = Guid.NewGuid().ToString();
        public string ActionID { get; set; }
        public string RoleID { get; set; }  
        public DateTime LastUpdateTime { get; set; }  = DateTime.Now;   
    }
    public partial class ActionInRole
    {
        public Actions Action { get; set; }
        public Roles Role { get; set; }
    }
}
