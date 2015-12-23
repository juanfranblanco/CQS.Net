using Infrastructure.Repository.SqlGenerator.Attributes;

namespace CQS.Demo.Business.Entities
{
    [StoredAs("Risks")]
    public class Risk 
    {
        [KeyProperty(Identity = true)]
        public int RiskId { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string PostCode { get; set; }
        public int PolicyId { get; set; }
    }
}