namespace E_BangDomain.RequestDtos.Shop
{
    public class CreateShopBranchDto
    {
        public string ShopID { get; set; }
        public string ShopCity { get; set; }
        public string ShopCountry { get; set; }
        public string ShopPostalCode { get; set; }
        public string ShopStreetName { get; set; }
        public bool IsMainShop { get; set; }

    }
}
