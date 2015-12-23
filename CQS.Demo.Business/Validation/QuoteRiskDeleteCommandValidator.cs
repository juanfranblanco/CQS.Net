using System.Collections;
using FluentValidation;

namespace CQS.Demo.Business.Validation
{
    public class QuoteRiskDeleteCommandValidator : AbstractValidator<QuoteRiskDeleteCommand> 
    {
        public QuoteRiskDeleteCommandValidator()
        {
            RuleFor(x => x.PolicyId).NotEmpty();
            GenericRules();
            RuleSet("Initial", GenericRules);
        }

        public void GenericRules()
        {
            RuleFor(x => x.RiskId).NotEmpty();
        }
    }
}
