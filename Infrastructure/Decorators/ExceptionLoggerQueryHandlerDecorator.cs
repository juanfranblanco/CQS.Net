using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentValidation;
using Infrastructure.CQS;
using Infrastructure.Logging;

namespace Infrastructure.Decorators
{
    public class ExceptionLoggerQueryHandlerDecorator<TQuery, TResult>
        : IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>

    {
        private readonly ILogger logger;
        private readonly IQueryHandler<TQuery, TResult> decorated;

        public ExceptionLoggerQueryHandlerDecorator(ILogger logger, IQueryHandler<TQuery, TResult> decorated)
        {
            this.logger = logger;
            this.decorated = decorated;
        }

        [DebuggerStepThrough]
        public async Task<TResult> HandleAsync(TQuery command)
        {
            try
            {
                return await this.decorated.HandleAsync(command);
            }
            catch (ValidationException ex)
            {
                logger.Warning(ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
                throw;
            }
        }
    }
}