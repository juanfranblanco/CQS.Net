using System.Collections.Generic;
using GLEAMS.Business.Entities;
using GLEAMS.Business.ValueObjects;
using Gleams.Infrastructure;
using Gleams.Infrastructure.SqlGenerator;

namespace GLEAMS.Business.Repositories
{
    public class RiskRepository : DataRepository<Risk>, IRiskRepository
    {
        public RiskRepository(IDefaultDbConnectionFactory dbConnectionFactory, ISqlGenerator<Risk> sqlGenerator)
            : base(dbConnectionFactory, sqlGenerator)
        {

        }

        public int GetCountByPolicyId(int policyId)
        {
            return this.GetCount(new { PolicyId = policyId });
        }

        public IEnumerable<Risk> GetByPolicyId(int policyId)
        {
            return this.GetWhere(new { PolicyId = policyId });
        }

        public IEnumerable<Risk> GetByPolicyId(int policyId, int pageNumber, int pageSize)
        {
            return this.GetWhere(new {PolicyId = policyId}, pageNumber, pageSize);
        }

        public PagedResult<Risk> GetByPolicyIdPagedResult(int policyId, int pageNumber, int pageSize)
        {
            return this.GetWherePagedResult(new {PolicyId = policyId}, pageNumber, pageSize);
        }

        public new bool Delete(int riskId)
        {
            return base.Delete(new {RiskId = riskId});
        }
    }
}