namespace E_BangDomain.RequestDtos.Shared
{
    public class ListDto<T> where T : class
    {
        public List<T> List { get; set; }
    }
}
