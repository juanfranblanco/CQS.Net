using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using FluentValidation;
using CQS.Demo.Business.Repositories;
using CQS.Demo.Business.Validation;
using Infrastructure;
using Infrastructure.Caching;
using Infrastructure.CQS;
using Infrastructure.CQS.Caching;
using Infrastructure.Decorators;
using Infrastructure.Logging;
using Infrastructure.Repository;
using Infrastructure.Repository.SqlGenerator;
using Infrastructure.SimpleInjection;
using SimpleInjector;
using SimpleInjector.Extensions;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;

namespace CQS.Demo.App_Start
{
    public class SimpleDependencyInjector : IServiceProvider
    {
        public readonly Container Container;

        public SimpleDependencyInjector()
        {
            Container = Bootstrap();
        }

        internal Container Bootstrap()
        {
            var container = new Container();

            // some container registrations

            InitializeContainer(container);

            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));

            container.Verify();
            return container;
        }

        private void RegisterFluentValidationValidators(Container container)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
            container.RegisterManyForOpenGeneric(typeof (IValidator<>), assemblies);
        }

        private void InitializeContainer(Container container)
        {
            RegisterFluentValidationValidators(container);
            RegisterValidatorBuilders(container);

            RegisterGenericRepositories(container);
            RegisterRepositories(container);

            RegisterServices(container);

            container.RegisterDecorator(
                typeof(ICommandHandler<>),
                typeof(ExceptionLoggerCommandHandlerDecorator<>));

            container.RegisterDecorator(
                typeof (ICommandHandler<>),
                typeof (ValidationCommandHandlerDecorator<>));

            container.RegisterDecorator(
                typeof(ICommandHandler<>),
                typeof(TransactionalCommandHandlerDecorator<>));

            container.RegisterDecorator(
              typeof(IQueryHandler<,>),
              typeof(CacheQueryHandlerDecorator<,>)
              );

            container.RegisterDecorator(
                typeof(IQueryHandler<,>),
                typeof(ExceptionLoggerQueryHandlerDecorator<,>)
                );

          

            container.RegisterWithContext<ILogger>(context => { return new NLogLogger(context.ImplementationType); });

            container.Register(typeof(IUnitOfWork), typeof(UnitOfWork));

            container.RegisterManyForOpenGeneric(typeof (ICommandHandler<>),
                AppDomain.CurrentDomain.GetAssemblies().ToList()
                );

            container.RegisterManyForOpenGeneric(
                typeof (IQueryHandler<,>), AppDomain.CurrentDomain.GetAssemblies().ToList()
                );
        }

        private static void RegisterServices(Container container)
        {
            
        }

        private static void RegisterRepositories(Container container)
        {
            container.RegisterSingle(typeof (IDbConnectionFactory), typeof (DbConnectionFactory));
        }


        private static void RegisterValidatorBuilders(Container container)
        {
            var webLifestyle = new WebRequestLifestyle();
            container.Register(typeof(IQuoteValidatonRuleService), typeof(QuoteValidatonRuleService), webLifestyle);
          
        }

        private static void RegisterGenericRepositories(Container container)
        {
//registering all open generic as singleton
            container.RegisterSingleOpenGeneric(
                typeof (ISqlDeleteGenerator<>),
                typeof (SqlDeleteGenerator<>));

            container.RegisterSingleOpenGeneric(
                typeof (ISqlInsertGenerator<>),
                typeof (SqlInsertGenerator<>));

            container.RegisterSingleOpenGeneric(
                typeof (ISqlUpdateGenerator<>),
                typeof (SqlUpdateGenerator<>));


            container.RegisterSingleOpenGeneric(
                typeof (ISqlWhereGenerator<>),
                typeof (SqlWhereGenerator<>));

            container.RegisterSingleOpenGeneric(
                typeof (ISqlSelectGenerator<>),
                typeof (SqlSelectGenerator<>));

            container.RegisterSingleOpenGeneric(
                typeof (IEntityMetadata<>),
                typeof (EntityMetadata<>));

            container.RegisterSingleOpenGeneric(typeof(IQueryCacheHandler<,>), typeof(QueryCacheHandler<,>) );

            container.RegisterSingleOpenGeneric(typeof(ICacheManager<>), typeof(CacheManager<>));

            container.RegisterSingleOpenGeneric(typeof (IGenericAsyncRepository<>), typeof (GenericRepository<>));
        }


        public object GetService(Type serviceType)
        {
            return ((IServiceProvider) Container).GetService(serviceType);
        }
    }
}