using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangDomain.Entities
{
    [Table("Product", Schema = "Product")]
    public partial class Product
    {
        [Key]
        public string ProductId { get; set; } = Guid.NewGuid().ToString();
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ShopID { get; set; }
        public DateTime LastModifiedTime { get; set; } = DateTime.UtcNow;
    }
    public partial class Product
    {
        public List<ProductInformations> ProductInformations { get; set; }
        public ProductPriceCount ProductCountPrice { get; set; }
    }

}
