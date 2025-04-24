namespace E_BangDomain.Pagination
{
    public class PaginationBuilder<T> where T : class 
    {
        private readonly PaginationBase<T> _instance = new();

        public PaginationBuilder<T> SetItems(List<T> Items)
        {
            _instance.Items = Items;
            return this;   
        }

        public PaginationBuilder<T> SetPageIndex(int Index)
        {
            _instance.PageIndex = Index;
            return this;
        }
        public PaginationBuilder<T> SetPageSize(int PageSize)
        {
            _instance.PageSize = PageSize;
            return this;
        }
        public PaginationBuilder<T> SetPageCount(int PageSize, int TotalItemsCount)
        {
            _instance.PageCount = Math.Floor((double)TotalItemsCount / PageSize);
            return this;
        }
        public PaginationBuilder<T> SetTotalItmesCount(int TotalItmesCount)
        {
            _instance.TotalItemsCount = TotalItmesCount;
            return this;
        }
        public PaginationBase<T> Build()
        {
            return _instance;
        }
        
    }
}
