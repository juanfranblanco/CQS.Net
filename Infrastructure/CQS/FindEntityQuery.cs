namespace Infrastructure.CQS
{
    public class FindEntityQuery<TEntity>:IQuery<TEntity>
    {
        public int Id { get; set; }
    }
}