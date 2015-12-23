using System.Threading.Tasks;
using CQS.Demo.Business.Entities;
using CQS.Demo.Business.Validation;
using Infrastructure.CQS;
using Infrastructure.Repository;

namespace CQS.Demo.Business.Services
{
    public class QuoteRiskUpdateHandler : ICommandHandler<QuoteRiskUpdateCommand>
    {
        private readonly IGenericAsyncRepository<Risk> riskRepository;

        public QuoteRiskUpdateHandler(IGenericAsyncRepository<Risk> riskRepository)
        {
            this.riskRepository = riskRepository;
        }

        public async Task HandleAsync(QuoteRiskUpdateCommand command)
        {
            await riskRepository.UpdateAsync(command);

        }
    }
}