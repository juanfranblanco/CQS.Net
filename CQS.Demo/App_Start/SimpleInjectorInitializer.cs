using FluentValidation.Mvc;

[assembly: WebActivator.PostApplicationStartMethod(typeof (CQS.Demo.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace CQS.Demo.App_Start
{
    public static class SimpleInjectorInitializer
    {
        public static void Initialize()
        {
            var injector = new SimpleDependencyInjector();
            FluentValidationModelValidatorProvider.Configure(
                provider => { provider.ValidatorFactory = new FluentValidatorFactory(injector); }
                );
        }
    }
}