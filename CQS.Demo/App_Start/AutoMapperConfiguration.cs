
using AutoMapper;
using CQS.Demo.Mappings;

[assembly: WebActivator.PostApplicationStartMethod(typeof(CQS.Demo.App_Start.AutoMapperConfiguration), "Configure")]
namespace CQS.Demo.App_Start
{
    public class AutoMapperConfiguration
    {
        public static void Configure()
        {
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<ViewModelToDomainMappingProfile>();
            });
        }
    }
}