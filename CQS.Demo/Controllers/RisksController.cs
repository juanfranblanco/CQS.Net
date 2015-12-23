using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using FluentValidation;
using FluentValidation.Mvc;
using CQS.Demo.Business.Entities;
using CQS.Demo.Business.Services;
using CQS.Demo.Business.Validation;
using CQS.Demo.Models;
using Infrastructure.CQS;
using Infrastructure.ValueObjects;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CQS.Demo.Controllers
{
    public class RisksController : Controller
    {
        private readonly IQueryHandler<FindPagedQuery<FindRisksForPolicyQuery, Risk>, PagedResult<Risk>> findRisksForPolicyHandler;
        private readonly ICommandHandler<QuoteRiskUpdateCommand> updateHandler;
        private readonly ICommandHandler<QuoteRiskCreateCommand> createCommandHandler;
        private readonly ICommandHandler<QuoteRiskDeleteCommand> deleteCommandHandler;


        private int policyId = 1; // temp value params are not in place yet

        public RisksController(IQueryHandler<FindPagedQuery<FindRisksForPolicyQuery, Risk>, PagedResult<Risk>> findRisksForPolicyHandler ,  ICommandHandler<QuoteRiskUpdateCommand> updateHandler, ICommandHandler<QuoteRiskCreateCommand> createCommandHandler, ICommandHandler<QuoteRiskDeleteCommand> deleteCommandHandler
            )
        {
            this.findRisksForPolicyHandler = findRisksForPolicyHandler;
            this.updateHandler = updateHandler;
            this.createCommandHandler = createCommandHandler;
            this.deleteCommandHandler = deleteCommandHandler;
        }

        public ActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryTokenAttribute]
        public async Task<ActionResult> Risks_Read([DataSourceRequest] DataSourceRequest request)
        {

                var query = new FindRisksForPolicyQuery() {PolicyId = policyId};

                var result =
                    await
                        findRisksForPolicyHandler.HandleAsync(new FindPagedQuery<FindRisksForPolicyQuery, Risk>()
                        {
                            Query = query,
                            PageNumber = request.Page,
                            PageSize = request.PageSize
                        });


                int total = result.Total;
                var risks = result.Result;

                //var viewModelCollection = Mapper.Map<IEnumerable<RiskViewModel>>(risks);

                return Json(new
                {
                    Data = risks,
                    Total = total
                });
  
        }

       
        [ValidateAntiForgeryTokenAttribute]
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Risks_Create([DataSourceRequest] DataSourceRequest request, QuoteRiskCreateCommand quoteRiskCreateCommand)
        {
            if (quoteRiskCreateCommand != null && ModelState.IsValid)
            {
                try
                {
                    await createCommandHandler.HandleAsync(quoteRiskCreateCommand);
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "System error, please contact admin if happens again");
                }
            }
            
           return Json(new[] { quoteRiskCreateCommand }.ToDataSourceResult(request, ModelState));
           
        }

        [ValidateAntiForgeryTokenAttribute]	
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Risks_Update([DataSourceRequest] DataSourceRequest request, [CustomizeValidator(RuleSet = "Initial")] QuoteRiskUpdateCommand quoteRiskViewModel)
        { 
               try{

                    if (quoteRiskViewModel != null && ModelState.IsValid)
                    {
                        quoteRiskViewModel.PolicyId = policyId;
                        await updateHandler.HandleAsync(quoteRiskViewModel);
                    }
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch
                {
                    ModelState.AddModelError("", "System error, please contact admin if happens again");
                }
            return Json(new[] { quoteRiskViewModel }.ToDataSourceResult(request, ModelState));
        }

        [ValidateAntiForgeryTokenAttribute]
        [AcceptVerbs(HttpVerbs.Post)]
        public async Task<ActionResult> Risks_Destroy([DataSourceRequest] DataSourceRequest request, [CustomizeValidator(RuleSet = "Initial")] QuoteRiskDeleteCommand quoteRiskDeleteCommand)
        {
            try
            {
                if (quoteRiskDeleteCommand != null && ModelState.IsValid)
                {
                    quoteRiskDeleteCommand.PolicyId = policyId;
                    await deleteCommandHandler.HandleAsync(quoteRiskDeleteCommand);
                }
            }
            catch (ValidationException ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            catch 
            {
                ModelState.AddModelError("", "System error, please contact admin if happens again");
            }
            return Json(new[] { quoteRiskDeleteCommand }.ToDataSourceResult(request, ModelState));
        }


        //public static Risk MapRiskFromViewModel(RiskViewModel riskViewModel, int policyId)
        //{
        //    var risk = Mapper.Map<Risk>(riskViewModel);
        //    risk.PolicyId = policyId;
        //    return risk;
        //}

    }
}
