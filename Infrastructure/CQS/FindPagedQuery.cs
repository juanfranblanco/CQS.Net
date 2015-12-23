using Infrastructure.ValueObjects;

namespace Infrastructure.CQS
{
    public class FindPagedQuery<TQuery, TEntity> : IQuery<PagedResult<TEntity>> where TEntity : new()
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public TQuery Query { get; set; }

    }
}