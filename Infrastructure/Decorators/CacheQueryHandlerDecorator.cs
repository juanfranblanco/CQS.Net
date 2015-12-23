using System.Reflection;
using System.Threading.Tasks;
using Infrastructure.Caching;
using Infrastructure.CQS;
using Infrastructure.CQS.Caching;

namespace Infrastructure.Decorators
{
    public class CacheQueryHandlerDecorator<TQuery, TResult> : IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly IQueryCacheHandler<TQuery, TResult> cacheHandler;
        private readonly IQueryHandler<TQuery, TResult> decorated;
        private readonly CachedAttribute cachedAttribute;

        public CacheQueryHandlerDecorator(IQueryCacheHandler<TQuery, TResult> cacheHandler,
            IQueryHandler<TQuery, TResult> decorated)
        {
            this.cacheHandler = cacheHandler;
            this.decorated = decorated;
            this.cachedAttribute = decorated.GetType().GetCustomAttribute<CachedAttribute>();
        }

        public async Task<TResult> HandleAsync(TQuery query)
        {
            if (cachedAttribute == null)
            {
                return await this.decorated.HandleAsync(query);
            }
            else
            {
                //TODO: query should be part of the key values + properties
                return await cacheHandler.GetAsync(decorated, query, cachedAttribute.Key, cachedAttribute.DurationInMinutes);
            }
        }
    }
}