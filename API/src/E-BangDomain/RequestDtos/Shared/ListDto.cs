namespace E_BangDomain.RequestDtos.Shared
{
    public class ListDto<T> where T : class
    {
        public string Id { get; set; } = string.Empty;
        public List<T> List { get; set; }
    }
}
