using System.Diagnostics;
using System.Threading.Tasks;
using Infrastructure.CQS;

namespace Infrastructure.Decorators
{
    public class TransactionalCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICommandHandler<TCommand> decorated;

        public TransactionalCommandHandlerDecorator(IUnitOfWork unitOfWork, ICommandHandler<TCommand> decorated)
        {
  
            this.unitOfWork = unitOfWork;
            this.decorated = decorated;
        }

        [DebuggerStepThrough]
        public async Task HandleAsync(TCommand command)
        {
            try
            {
                unitOfWork.BeginTransaction();
                await decorated.HandleAsync(command);
                unitOfWork.CommitTransaction();
            }
            catch
            {
                unitOfWork.RollBackTransaction();
                throw;
            }
        }

    }
}