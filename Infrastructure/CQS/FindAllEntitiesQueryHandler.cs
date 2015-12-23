using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.Repository;

namespace Infrastructure.CQS
{
    public class FindAllEntitiesQueryHandler<TEntity> :
        IQueryHandler<FindAllEntitiesQuery<TEntity>, IEnumerable<TEntity>>  where TEntity : new()
    {
        protected IGenericAsyncRepository<TEntity> GenericRepository { get; private set; }

        public FindAllEntitiesQueryHandler(IGenericAsyncRepository<TEntity> genericRepository)
        {
            this.GenericRepository = genericRepository;
        }

        public async Task<IEnumerable<TEntity>> HandleAsync(FindAllEntitiesQuery<TEntity> query)
        {
            return await GenericRepository.GetAllAsync();
        }
    }
}