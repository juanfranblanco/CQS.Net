using System.Collections.Generic;
using System.Linq;
using CQS.Demo.Business.Entities;
using Infrastructure.CQS;

namespace CQS.Demo.Business.Validation
{
    /// <summary>
    /// To share the rules between RiskUpdate and RiskCreate (normally commands will be different for either situation) 
    /// we have a base class that initialises the commnon rules
    /// </summary>
    public class QuoteValidatonRuleService : IQuoteValidatonRuleService
    {
        private readonly IQueryHandler<FindAllEntitiesQuery<ValidationRule>, IEnumerable<ValidationRule>> validationRuleHandler;
        private IEnumerable<ValidationRule> validationRules;
        public QuoteValidatonRuleService(
            IQueryHandler<FindAllEntitiesQuery<ValidationRule>, IEnumerable<ValidationRule>> validationRuleHandler)
        {
            this.validationRuleHandler = validationRuleHandler;
        }
            
        /// <summary>
        /// Stored the value into a temp variable, this valiation service is stored for per web request so we ensure the same validation rules are applied for a request
         /// Using Fluent validation rules are set in the constructor, so the service is synchronous
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ValidationRule> GetValidationRules()
        {
            if (validationRules == null)
            { 
                var task = validationRuleHandler.HandleAsync(new FindAllEntitiesQuery<ValidationRule>());
                task.Wait();
                validationRules = task.Result;
            }
            return validationRules;
        }

        private IEnumerable<ValidationRule> GetQuoteValidationRules()
        {
            return GetValidationRules().Where(x => x.EntityName == "Risk" && x.PolicyQuote == "Q");
        } 

        public bool IsAddress1Required()
        {
            return IsRequiredQuoteRisk("Address1");
        }

        private bool IsRequiredQuoteRisk(string fieldName)
        {
            var field = GetQuoteValidationRules().Where(x => x.FieldName == fieldName);
            var firstOrDefault = field.FirstOrDefault();
            return firstOrDefault != null && firstOrDefault.Required;
        }
    }
}