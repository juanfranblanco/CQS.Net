using CQS.Demo.Business.Entities;
using Infrastructure.CQS;

namespace CQS.Demo.Business.Validation
{
    public class QuoteRiskCreateCommand : Risk, ICommand
    {

    }

    public class PolicyCreateCommand : ICommand
    {
        /// <summary>
        /// Returned policy Id
        /// </summary>
        public int PolicyId { get; set; }
        public string PolicyName { get; set; }
    }
}