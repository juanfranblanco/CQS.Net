using Infrastructure.Repository.SqlGenerator.Attributes;

namespace CQS.Demo.Business.Entities
{
    [StoredAs("ValidationRule")]
    public class ValidationRule
    {
        public const string POLICY = "P";
        public const string QUOTE = "Q";

        [KeyProperty(Identity = true)]
        public int ValidationRuleId { get; set; }
        public bool Required { get; set; }
        public string FieldName { get; set; }
        public string EntityName { get; set; }
        public string PolicyQuote { get; set; }
    }
}