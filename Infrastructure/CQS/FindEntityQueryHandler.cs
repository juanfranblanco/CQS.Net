using System.Threading.Tasks;
using Infrastructure.Repository;

namespace Infrastructure.CQS
{
    public class FindEntityQueryHandler<TEntity> : IQueryHandler<FindEntityQuery<TEntity>, TEntity> where TEntity : new()
    {
        protected IGenericAsyncRepository<TEntity> GenericRepository { get; private set; }

        public FindEntityQueryHandler(IGenericAsyncRepository<TEntity> genericRepository)
        {
            this.GenericRepository = genericRepository;
        }

        public async Task<TEntity> HandleAsync(FindEntityQuery<TEntity> query)
        {
            return await GenericRepository.GetFirstAsync(new {query.Id});
        }        
    }
}