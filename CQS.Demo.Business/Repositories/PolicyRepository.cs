using GLEAMS.Business.Entities;
using Gleams.Infrastructure;
using Gleams.Infrastructure.SqlGenerator;

namespace GLEAMS.Business.Repositories
{
    public class PolicyRepository : DataRepository<Policy>
    {
        public PolicyRepository(IDefaultDbConnectionFactory dbConnectionFactory, ISqlGenerator<Policy> sqlGenerator)
            : base(dbConnectionFactory, sqlGenerator)
        
        {
        
        }
    }
}