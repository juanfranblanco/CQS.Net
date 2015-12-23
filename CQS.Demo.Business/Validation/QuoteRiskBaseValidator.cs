using FluentValidation;
using CQS.Demo.Business.Entities;

namespace CQS.Demo.Business.Validation
{

    public class PolicyCreateValidator : AbstractValidator<PolicyCreateCommand>
    {
        public PolicyCreateValidator()
        {
            RuleFor(x => x.PolicyName).Length(2, 100).NotEmpty();
        }   
    }

    public class QuoteRiskBaseValidator<T>:AbstractValidator<T> where T: Risk
    {
        private readonly IQuoteValidatonRuleService quoteValidatonRuleService;

        //Quotation validation rule service is injected to get the rules from the service / store
        public QuoteRiskBaseValidator(IQuoteValidatonRuleService quoteValidatonRuleService)
        {
            this.quoteValidatonRuleService = quoteValidatonRuleService;
            GenericRules();
        }

        public void GenericRules()
        {
            if (quoteValidatonRuleService.IsAddress1Required())
            {
                RuleFor(x => x.Address1).Length(5, 10).NotEmpty();
            }

        }
    }
}