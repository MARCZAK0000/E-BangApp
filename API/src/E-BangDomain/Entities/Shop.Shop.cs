using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangDomain.Entities
{
    [Table("Shop", Schema = "Shop")]
    public partial class Shop
    {
        [Key]
        public string ShopId {  get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string ShopName { get; set; } 
        public string ShopDescription { get; set; }
        public int ShopTypeId { get; set; }
        public DateTime LastModifiedTime { get; set; }
    }

    public partial class Shop
    {
        public ShopType ShopType { get; set; }
        public List<Product> Products { get; set; }
        public List<ShopStaff> ShopStaff {  get; set; }
        public List<ShopBranchesInformations> ShopAddressInfromations { get; set; }
    }
}
