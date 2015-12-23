using System.Collections.Generic;

namespace Infrastructure.CQS
{
    public class FindAllEntitiesQuery<TEntity> : IQuery<IEnumerable<TEntity>>
    {

    }
}