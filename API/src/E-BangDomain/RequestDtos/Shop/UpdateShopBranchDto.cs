namespace E_BangDomain.RequestDtos.Shop
{
    public class UpdateShopBranchDto : CreateShopBranchDto
    {
        public string BranchId { get; set; } // Unique identifier for the shop branch to be updated
    
    }
}
