using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangDomain.Entities
{
    [Table("ShopType", Schema = "Shop")]
    public class ShopType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string ShopTypeName { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }
}
