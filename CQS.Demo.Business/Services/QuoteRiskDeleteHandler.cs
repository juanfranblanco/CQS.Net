using System.Threading.Tasks;
using CQS.Demo.Business.Entities;
using CQS.Demo.Business.Validation;
using Infrastructure.CQS;
using Infrastructure.Repository;

namespace CQS.Demo.Business.Services
{
    public class QuoteRiskDeleteHandler : ICommandHandler<QuoteRiskDeleteCommand>
    {
        private readonly IGenericAsyncRepository<Risk> riskRepository;

        public QuoteRiskDeleteHandler(IGenericAsyncRepository<Risk> riskRepository)
        {
            this.riskRepository = riskRepository;
        }

        public async Task HandleAsync(QuoteRiskDeleteCommand command)
        {
            await riskRepository.DeleteAsync(command.PolicyId);

        }
    }
}