using System.Collections;
using Infrastructure.Repository.SqlGenerator.Attributes;

namespace CQS.Demo.Business.Entities
{
    [StoredAs("Policies")]
    public class Policy
    {
        [KeyProperty(Identity = true)]
        public int PolicyId { get; set; }

        public string Name { get; set; }
    }
}
