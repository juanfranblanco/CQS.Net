using System;
using System.Diagnostics;
using System.Threading.Tasks;
using FluentValidation;
using Infrastructure.CQS;
using Infrastructure.Logging;

namespace Infrastructure.Decorators
{
    public class ExceptionLoggerCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly ILogger logger;
        private readonly ICommandHandler<TCommand> decorated;

        public ExceptionLoggerCommandHandlerDecorator(ILogger logger, ICommandHandler<TCommand> decorated)
        {
            this.logger = logger;
            this.decorated = decorated;
        }

        [DebuggerStepThrough]
        public async Task HandleAsync(TCommand command)
        {
            try
            {
                await this.decorated.HandleAsync(command);
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