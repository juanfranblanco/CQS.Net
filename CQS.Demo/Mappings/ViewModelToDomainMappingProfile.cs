using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using CQS.Demo.Business.Entities;
using CQS.Demo.Models;

namespace CQS.Demo.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        protected override void Configure()
        {
           // Mapper.CreateMap<RiskViewModel, Risk>();
          
        }
    }


}