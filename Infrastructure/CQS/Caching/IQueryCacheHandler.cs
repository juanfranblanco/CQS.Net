using System.Threading.Tasks;

namespace Infrastructure.CQS.Caching
{
    public interface IQueryCacheHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {

        Task<TResult> GetAsync(IQueryHandler<TQuery, TResult> handler, TQuery query, string key, int cacheDurationInMinutes);
    }
}