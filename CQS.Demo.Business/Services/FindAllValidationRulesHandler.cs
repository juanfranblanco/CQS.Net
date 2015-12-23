using CQS.Demo.Business.Entities;
using Infrastructure.Caching;
using Infrastructure.CQS;
using Infrastructure.Repository;

namespace CQS.Demo.Business.Services
{
    [Cached(DurationInMinutes = 15, Key= "AllValidationRules")]
    public class FindAllValidationRulesHandler : FindAllEntitiesQueryHandler<ValidationRule>
    {
        public FindAllValidationRulesHandler(IGenericAsyncRepository<ValidationRule> genericRepository) : base(genericRepository)
        {
        }
    }
}