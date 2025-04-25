using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangDomain.Entities
{
    [Table("ShopBranchesInformations", Schema = "Shop")]
    public partial class ShopBranchesInformations
    {
        public string ShopBranchId { get; set; } = Guid.NewGuid().ToString();
        public string ShopID { get; set; }
        public string ShopCity { get; set; }
        public string ShopCountry { get; set; }
        public string ShopPostalCode { get; set; }
        public string ShopStreetName  { get; set; }
        public bool IsMainShop { get; set; } = false;
        public DateTime LastModifiedTime { get; set; } = DateTime.Now;
    }
    public partial class ShopBranchesInformations
    {
        public Product Product { get; set; }
    }
}
