using System.Threading.Tasks;
using Infrastructure.Caching;

namespace Infrastructure.CQS.Caching
{
    public class QueryCacheHandler<TQuery, TResult> : IQueryCacheHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        private readonly ICacheManager<TResult> cacheManager;
    
        public QueryCacheHandler(ICacheManager<TResult> cacheManager)
        {
            this.cacheManager = cacheManager;
        }

        public async Task<TResult> GetAsync(IQueryHandler<TQuery, TResult> handler, TQuery query, string key, int cacheDurationInMinutes)
        {
            return await cacheManager.AddOrGetExisting(key, () => handler.HandleAsync(query), cacheDurationInMinutes);  
        }
    }
}