namespace E_BangApplication.Cache.Base
{
    public interface ICacheItem
    {
        /// <summary>
        /// Gets or sets the unique key used to identify cached items.
        /// </summary>
        string CacheKey { get; set; }
       
    }
    public interface ICacheDuration
    {
        /// <summary>
        /// Gets or sets the duration for which cached data remains valid.
        /// </summary>
        TimeSpan CacheDuration { get; set; }
    }
    public interface ICachePaginationBase
    {
        /// <summary>
        /// Gets or sets the current page number in a paginated collection.
        /// </summary>
        int PageNumber { get; set; }   
        /// <summary>
        /// Gets or sets the number of items to display per page in a paginated list.
        /// </summary>
        int PageSize { get; set; }
    }
}
