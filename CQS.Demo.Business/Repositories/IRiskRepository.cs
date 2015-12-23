using GLEAMS.Business.Entities;
using GLEAMS.Business.ValueObjects;

namespace GLEAMS.Business.Repositories
{
    public interface IRiskRepository
    {
        PagedResult<Risk> GetByPolicyIdPagedResult(int policyId, int pageNumber, int pageSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        bool Insert(Risk instance);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        bool Update(Risk instance);

        bool Delete(int riskId);
    }
}