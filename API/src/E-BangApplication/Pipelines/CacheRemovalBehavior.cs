using E_BangApplication.Cache;
using Microsoft.Extensions.Caching.Distributed;
using MyCustomMediator.Deleagate;
using MyCustomMediator.Interfaces;

public class CacheRemovalBehavior<TRequest, TResponse> : IPipeline<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : class, new()
{
    private readonly IDistributedCache _cache;
    public CacheRemovalBehavior(IDistributedCache cache) => _cache = cache;

    public async Task<TResponse> SendToPipeline(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken token)
    {
        if (request is ICacheRemovable cacheDisposable)
        {
            string cacheKey = cacheDisposable.CacheKey;
            byte[]? cachedResponse = await _cache.GetAsync(cacheKey, token);
            if (cachedResponse == null)
            {
                return await next();
            }
            await _cache.RemoveAsync(cacheKey, token);
        }
        return await next();
    }
}