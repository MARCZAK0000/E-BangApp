using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangDomain.Entities
{
    [Table(name: "Action", Schema = "Security")]
    public partial class Actions
    {
        [Key]
        public string ActionID { get; set; } = Guid.NewGuid().ToString();
        public string ActionName { get; set; }  
        public string ActionDescription { get; set; }
        public DateTime LastUpdateTime = DateTime.Now;
    }
    public partial class Actions
    {
        public List<ActionInRole> ActionInRoles { get; set; }
    }
}
