using System;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Infrastructure.Caching
{
    public class CacheManager<TResult> : ICacheManager<TResult> where TResult : class
    {
        private static readonly ObjectCache Cache = MemoryCache.Default;

        public async Task<TResult> AddOrGetExisting(string key, Func<Task<TResult>> factoryMethod, int cacheDurationInMinutes)
        {
            var policy = GetAbsoluteExpirationPolicy(cacheDurationInMinutes);

            if (Cache.Contains(key))
            {
                return Get(key);
            }
            
            var result = await factoryMethod();

            var item = Cache.AddOrGetExisting(key, result, policy);

            return (TResult)item ?? (TResult)result;
        }

        private CacheItemPolicy GetAbsoluteExpirationPolicy(int cacheDurationInMinutes)
        {
            var policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(cacheDurationInMinutes)
            };
            return policy;
        }

        public TResult Get(string key)
        {
            return (TResult)Cache.Get(key);
        }

        public void Remove(string key)
        {
            Cache.Remove(key);
        }
    }
}