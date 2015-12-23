using System.Threading.Tasks;

namespace Infrastructure.CQS
{
    public interface IQueryHandler<TQuery, TResult>
        where TQuery : IQuery<TResult> 
         
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}