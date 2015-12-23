using System.Collections.Generic;
using CQS.Demo.Business.Entities;
using Infrastructure.CQS;

namespace CQS.Demo.Business.Validation
{
    public class FindRisksForPolicyQuery : IQuery<IEnumerable<Risk>>
    {
        public int PolicyId { get; set; }
    }
}