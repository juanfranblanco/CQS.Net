using FluentValidation;

namespace CQS.Demo.Business.Validation
{
    public class QuoteRiskUpdateCommandValidator : QuoteRiskBaseValidator<QuoteRiskUpdateCommand>
    {
        public QuoteRiskUpdateCommandValidator(IQuoteValidatonRuleService quoteValidatonRuleService) : base(quoteValidatonRuleService)
        {
            
            RuleFor(x => x.PolicyId).GreaterThan(100);

            RuleSet("Initial", GenericRules);
        }
    }
}