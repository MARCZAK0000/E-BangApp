using System.ComponentModel.DataAnnotations.Schema;

namespace E_BangDomain.Entities
{
    [Table("UserAddress", Schema = "Account")]
    public partial class UserAddress
    {
        public string UserID { get; set; }
        public string City { get; set; }
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public DateTime LastUpdateTime { get; set; } = DateTime.Now;
    }
    public partial class UserAddress
    {
        //public string FullAddress { get => FullAddress; private set => _ = $"{City}, {StreetNumber}, {PostalCode}, {Country}"; }
        public Users User { get; set; }
    }
}
