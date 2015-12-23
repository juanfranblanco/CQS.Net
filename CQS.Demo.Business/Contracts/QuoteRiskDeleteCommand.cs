using Infrastructure.CQS;

namespace CQS.Demo.Business.Validation
{
    public class QuoteRiskDeleteCommand : ICommand
    {
        public int PolicyId { get; set; }
        public int RiskId { get; set; }
    }
}