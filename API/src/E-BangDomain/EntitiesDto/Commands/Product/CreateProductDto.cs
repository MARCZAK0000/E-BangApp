namespace E_BangDomain.EntitiesDto.Commands.Product
{
    public class CreateProductDto
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ShopId { get; set; }
        public decimal Price { get; set; }

        public int ProductCount { get; set; }
    }
}
