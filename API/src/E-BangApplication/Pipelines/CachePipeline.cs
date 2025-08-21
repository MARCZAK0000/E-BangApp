using E_BangApplication.Cache;
using E_BangDomain.Cache;
using E_BangDomain.ResponseDtos.Account;
using Microsoft.Extensions.Caching.Distributed;
using MyCustomMediator.Deleagate;
using MyCustomMediator.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace E_BangApplication.Pipelines
{
    public sealed class CachingBehavior<TRequest, TResponse> : IPipeline<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class, new()
    {
        private readonly Func<bool, ICacheStrategy> _cacheStrategyFactory; 
        public CachingBehavior(Func<bool, ICacheStrategy> cacheStrategyFactory)
        {
            _cacheStrategyFactory = cacheStrategyFactory;
        }
        /// <summary>
        /// Processes the specified request through the pipeline, applying caching or cache removal strategies if
        /// applicable.
        /// </summary>
        /// <remarks>If the request implements <see cref="ICacheable"/>, the method applies a caching
        /// strategy to store or retrieve the response from the cache. If the request implements <see
        /// cref="ICacheRemovable"/>, the method applies a cache removal strategy to invalidate the relevant cache
        /// entries. If the request does not implement either interface, the method simply invokes the next step in the
        /// pipeline.</remarks>
        /// <param name="request">The request to be processed. Must implement <see cref="ICacheable"/> or <see cref="ICacheRemovable"/> to
        /// trigger caching behavior.</param>
        /// <param name="next">The delegate representing the next step in the pipeline.</param>
        /// <param name="token">A <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the response produced by the
        /// pipeline.</returns>
        public async Task<TResponse> SendToPipeline(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken token)
        {
            // Add to Cache
            if (request is ICacheable cache)
            {
                var result = _cacheStrategyFactory.Invoke(true);
                return await result.Handle(request, async () => 
                {
                    return await next();    
                }, token);
            }
            // Remove from Cache
            if(request is ICacheRemovable cacheDisposable)
            {
                var result = _cacheStrategyFactory.Invoke(false);
                return await result.Handle(request, async () =>
                {
                    return await next();
                }, token);
            }

            return await next();
        }
    }
}