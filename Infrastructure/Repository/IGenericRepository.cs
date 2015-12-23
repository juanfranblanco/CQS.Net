using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : new()
    {
        bool Delete(object key);
      
        IEnumerable<TEntity> GetAll();

        TEntity GetFirst(object filters);
       
        IEnumerable<TEntity> GetWhere(object filters);
       
        bool Insert(TEntity instance);

        bool Update(TEntity instance);
      
        IEnumerable<TEntity> GetWhere(object filters, int pageNumber, int pageSize);
        Task<IEnumerable<TEntity>> GetWhereAsync(object filters, int pageNumber, int pageSize);
        int GetCount(object filters);
        
    }
}