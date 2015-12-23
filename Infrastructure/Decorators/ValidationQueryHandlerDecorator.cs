using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentValidation;
using Infrastructure.CQS;

namespace Infrastructure.Decorators
{
    public class ValidationQueryHandlerDecorator<TQuery, TResult>
        : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>

    {
        private readonly IServiceProvider provider;
        private readonly IValidator<TQuery> validator;
        private readonly  IQueryHandler<TQuery, TResult> decorated;

        public ValidationQueryHandlerDecorator(IValidator<TQuery> validator, IQueryHandler<TQuery, TResult> decorated)
        {
            this.validator = validator;
            this.decorated = decorated;
        }

        [DebuggerStepThrough]
        public async Task<TResult> HandleAsync(TQuery query)
        {
            await validator.ValidateAndThrowAsync(query);

            return await this.decorated.HandleAsync(query);
        }
    }
}