namespace CQS.Demo.Business.Validation
{
    public class QuoteRiskCreateValidator : QuoteRiskBaseValidator<QuoteRiskCreateCommand>
    {
        public QuoteRiskCreateValidator(IQuoteValidatonRuleService quoteValidatonRuleService) : base(quoteValidatonRuleService)
        {
        }
    }
}