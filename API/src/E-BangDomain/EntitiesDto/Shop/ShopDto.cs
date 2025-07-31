using System.ComponentModel.DataAnnotations;

namespace E_BangDomain.EntitiesDto.Shop
{
    public class ShopDto
    {
        public string ShopId { get; set; } 
        public string ShopName { get; set; }
        public string ShopDescription { get; set; }
        public int ShopTypeId { get; set; }
    }
}
