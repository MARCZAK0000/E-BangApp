using E_BangDomain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangDomain.Entities
{
    [Table("ProductInformations",Schema = "Product")]
    public partial class ProductInformations
    {
        public string ProductID { get; set; }
        public string? ProductPath { get; set; }
        public EProductInformationsType InformationsType { get; set; }
        public DateTime LastModifiedTime { get; set; } = DateTime.UtcNow;
    }
    public partial class ProductInformations
    {
        public Product Product { get; set; }
    }
}
