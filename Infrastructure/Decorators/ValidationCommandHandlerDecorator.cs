using System.Diagnostics;
using System.Threading.Tasks;
using FluentValidation;
using Infrastructure.CQS;

namespace Infrastructure.Decorators
{
    public class ValidationCommandHandlerDecorator<TCommand>
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        private readonly IValidator<TCommand> validator;
        private readonly ICommandHandler<TCommand> decorated;

        public ValidationCommandHandlerDecorator(IValidator<TCommand> validator, ICommandHandler<TCommand> decorated)
        {
           
            this.validator = validator;
            this.decorated = decorated;
        }

        [DebuggerStepThrough]
        public async Task HandleAsync(TCommand command)
        {
           await validator.ValidateAndThrowAsync(command);

            await this.decorated.HandleAsync(command);
        }
    }
}