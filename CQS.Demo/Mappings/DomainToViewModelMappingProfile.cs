using AutoMapper;
using CQS.Demo.Business.Entities;
using CQS.Demo.Models;

namespace CQS.Demo.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        protected override void Configure()
        {
           // Mapper.CreateMap<Risk, RiskViewModel>();

        }

    }
}