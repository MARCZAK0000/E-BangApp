using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangDomain.Entities
{
    [Table("ShopType", Schema = "Shop")]
    public partial class ShopType
    {
        [Key]
        public int ShopTypeId { get; set; }
        [Required]
        public string ShopTypeName { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
    public partial class ShopType
    {
        public List<Shop> Shops { get; set; }
    }
}
