using System.Threading.Tasks;
using Infrastructure.Repository;
using Infrastructure.ValueObjects;

namespace Infrastructure.CQS
{
    public class FindPagedQueryGenericHandler<TEntity, TQuery> :
        IQueryHandler<FindPagedQuery<TQuery, TEntity>, PagedResult<TEntity>> where TEntity : new()
    {
        protected IGenericAsyncRepository<TEntity> GenericRepository { get; private set; }

        public FindPagedQueryGenericHandler(IGenericAsyncRepository<TEntity> genericRepository)
        {
            this.GenericRepository = genericRepository;
        }

       
        public virtual async Task<PagedResult<TEntity>> HandleAsync(FindPagedQuery<TQuery, TEntity> query)
        {
            return await GenericRepository.GetWherePagedResultAsync(query.Query, query.PageNumber, query.PageSize);
        }
    }
}