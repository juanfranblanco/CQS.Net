using System.Threading.Tasks;

namespace Infrastructure.CQS
{
    public interface ICommandHandler<in TCommand>
        where TCommand : ICommand
    {
        Task HandleAsync(TCommand command);
    }
}