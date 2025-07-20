using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangDomain.Entities
{
    [Table("ProductPrice", Schema = "Product")]
    public partial class ProductPrice
    {
        public string ProductPriceId { get; set; } = Guid.NewGuid().ToString();
        public decimal Price { get; set; }
        public string ProductID { get; set; }
        public int ProductCount { get; set; }
        public DateTime LastModifiedTime { get; set; } = DateTime.UtcNow;
    }
    public partial class ProductPrice
    {
        public Product Product { get; set; }
    }
}
