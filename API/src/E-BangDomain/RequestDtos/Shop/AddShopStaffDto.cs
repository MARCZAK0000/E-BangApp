namespace E_BangDomain.RequestDtos.Shop
{
    public class AddShopStaffDto
    {
        public string EmailAddress { get; set; }   
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
        public bool CanCreate { get; set; }
        public int ActionLevel { get; set; }   
    }
}
