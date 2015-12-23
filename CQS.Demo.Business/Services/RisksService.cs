using GLEAMS.Business.Entities;
using GLEAMS.Business.Repositories;
using GLEAMS.Business.ValueObjects;

namespace GLEAMS.Business.Services
{
    public class RisksService
    {
        private readonly IRiskRepository riskRepository;

        public RisksService(IRiskRepository riskRepository)
        {
            this.riskRepository = riskRepository;
        }

        public PagedResult<Risk> GetPagedResult(int policyId, int pageNumber, int pageSize)
        {
            return riskRepository.GetByPolicyIdPagedResult(policyId, pageNumber, pageSize);
        }

        public Risk Create(Risk risk)
        {
            riskRepository.Insert(risk);
            return risk;
        }

        public Risk Update(Risk risk)
        {
            riskRepository.Update(risk);
            return risk;
        }

        public void Delete(Risk risk)
        {
            riskRepository.Delete(risk.RiskId);
        }
    }
}
