using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Repository;

namespace Infrastructure.CQS
{
    public class FindQueryGenericHandler<TEntity, TQuery> :
        IQueryHandler<TQuery, IEnumerable<TEntity>> where TEntity : new() where TQuery : IQuery<IEnumerable<TEntity>>
    {
        protected IGenericAsyncRepository<TEntity> GenericRepository { get; private set; }

        public FindQueryGenericHandler(IGenericAsyncRepository<TEntity> genericRepository)
        {
            this.GenericRepository = genericRepository;
        }

        public virtual async Task<IEnumerable<TEntity>> HandleAsync(TQuery query)
        {
            return await GenericRepository.GetWhereAsync((object) query);
        }
    }
}