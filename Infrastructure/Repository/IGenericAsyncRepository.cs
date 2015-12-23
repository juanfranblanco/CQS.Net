using System.Collections.Generic;
using System.Threading.Tasks;
using Infrastructure.ValueObjects;

namespace Infrastructure.Repository
{
    public interface IGenericAsyncRepository<TEntity> where TEntity : new()
    {
        Task<bool> DeleteAsync(object key);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetFirstAsync(object filters);
        Task<IEnumerable<TEntity>> GetWhereAsync(object filters);
        Task<bool> InsertAsync(TEntity instance);
        Task<bool> UpdateAsync(TEntity instance);
        Task<int> GetCountAsync(object filters);
        Task<PagedResult<TEntity>> GetWherePagedResultAsync(object filters, int pageNumber, int pageSize);
    }
}