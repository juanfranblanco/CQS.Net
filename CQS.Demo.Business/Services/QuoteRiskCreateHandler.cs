using System;
using System.Threading.Tasks;
using CQS.Demo.Business.Entities;
using CQS.Demo.Business.Validation;
using Infrastructure.CQS;
using Infrastructure.Repository;

namespace CQS.Demo.Business.Services
{


    public class PolicyCreateHandler : ICommandHandler<PolicyCreateCommand>
    {
        private readonly IGenericAsyncRepository<Policy> policyGenericRepository;

        public PolicyCreateHandler(IGenericAsyncRepository<Policy> policyGenericRepository)
        {
            this.policyGenericRepository = policyGenericRepository;
        }

        public async Task HandleAsync(PolicyCreateCommand command)
        {
            var policy = new Policy() {Name = command.PolicyName};
            var success =  await  policyGenericRepository.InsertAsync(policy);
            command.PolicyId = policy.PolicyId;
        }
    }


    public class QuoteRiskCreateHandler : ICommandHandler<QuoteRiskCreateCommand>
    {
        private readonly IGenericAsyncRepository<Risk> riskRepository;

        public QuoteRiskCreateHandler(IGenericAsyncRepository<Risk> riskRepository)
        {
            this.riskRepository = riskRepository;
        }

        public async Task HandleAsync(QuoteRiskCreateCommand command)
        {
            await riskRepository.InsertAsync(command);
            //uncomment to see it getting handled by the decorator, rollbacked and logged
            //throw new Exception("An exception");

        }
    }
}