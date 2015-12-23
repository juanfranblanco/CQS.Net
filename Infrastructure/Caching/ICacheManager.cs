using System;
using System.Threading.Tasks;

namespace Infrastructure.Caching
{
    public interface ICacheManager<TResult>
    {
        Task<TResult> AddOrGetExisting(string key, Func<Task<TResult>> factoryMethod, int cacheDurationInMinutes);
        TResult Get(string key);
        void Remove(string key);
    }
}