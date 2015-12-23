using CQS.Demo.Business.Entities;
using CQS.Demo.Business.Validation;
using Infrastructure.CQS;
using Infrastructure.Repository;

namespace CQS.Demo.Business.Services
{
    public class FindRisksForPolicyHandler : FindPagedQueryGenericHandler<Risk, FindRisksForPolicyQuery>
    {
        public FindRisksForPolicyHandler(IGenericAsyncRepository<Risk> genericRepository)
            : base(genericRepository)
        {
        }
    }


}