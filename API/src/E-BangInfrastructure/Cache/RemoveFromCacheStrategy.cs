using E_BangApplication.Cache;
using E_BangDomain.Cache;
using Microsoft.Extensions.Caching.Distributed;

namespace E_BangInfrastructure.Cache
{
    public sealed class RemoveFromCacheStrategy: ICacheStrategy
    {
        private readonly IDistributedCache _cache;
        public RemoveFromCacheStrategy(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, Func<Task<TResponse>> nextAction, CancellationToken token)
        {
            ICacheRemovable? cacheRemovable = request as ICacheRemovable;
            if (cacheRemovable == null)
            {
                return await nextAction();
            }
            string cacheKey = cacheRemovable.CacheKey;
            byte[]? cachedResponse = await _cache.GetAsync(cacheKey, token);
            if (cachedResponse == null)
            {
                return await nextAction();
            }
            await _cache.RemoveAsync(cacheKey, token);
            return await nextAction();
        }
    }
}
