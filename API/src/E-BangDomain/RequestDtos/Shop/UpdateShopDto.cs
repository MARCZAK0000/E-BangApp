using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_BangDomain.RequestDtos.Shop
{
    public class UpdateShopDto : CreateShopDto
    {
        public string ShopId { get; set; }
    }
}
