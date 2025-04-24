namespace E_BangDomain.Pagination
{
    public class PaginationBase<T> where T : class
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public double PageCount { get; set; }
        public int TotalItemsCount { get; set; }

    }
}
