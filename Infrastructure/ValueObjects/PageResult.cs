using System.Collections.Generic;

namespace Infrastructure.ValueObjects
{
    public class PagedResult<TEntity> where TEntity: new()
    {
        public int Total { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public IEnumerable<TEntity> Result { get; set; } 
    }
}
