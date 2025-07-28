using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangDomain.Entities
{
    [Table("ShopStaff", Schema = "Shop")]
    public partial class ShopStaff
    {
        public string ShopStaffId { get; set; } = Guid.NewGuid().ToString();    
        public string ShopId { get; set; }
        public string AccountId { get; set; }
        public int ActionLevel { get; set; } 
    }

    public partial class ShopStaff
    {
        public virtual Shop Shop { get; set; }
        public virtual Users Users { get; set; }
    }
}
