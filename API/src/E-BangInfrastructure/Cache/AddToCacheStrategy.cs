using E_BangApplication.Cache;
using E_BangDomain.Cache;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_BangInfrastructure.Cache
{
    public class AddToCacheStrategy: ICacheStrategy
    {
        private readonly IDistributedCache _cache;

        public AddToCacheStrategy(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<TResponse> Handle<TRequest, TResponse>(TRequest request, Func<Task<TResponse>> nextAction, CancellationToken token)
        {
            ICacheable? cache = request as ICacheable;
            if (cache == null)
            {
                return await nextAction();
            }
            string cacheKey = cache!.CacheKey;
            byte[]? cachedResponse = await _cache.GetAsync(cacheKey, token);
            if (cachedResponse == null)
            {
                TResponse response = await nextAction();
                string json = JsonSerializer.Serialize(response);
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                await _cache.SetAsync(cacheKey, bytes, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = cache.CacheDuration
                }, token);
                return response;
            }
            string jsonResponse = Encoding.UTF8.GetString(cachedResponse);
            TResponse cachedResult = JsonSerializer.Deserialize<TResponse>(jsonResponse)!;
            return cachedResult;

        }
    }
}
