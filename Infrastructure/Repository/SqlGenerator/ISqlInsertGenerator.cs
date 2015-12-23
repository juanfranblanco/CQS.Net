namespace Infrastructure.Repository.SqlGenerator
{
    public interface ISqlInsertGenerator<TEntity> where TEntity : new()
    {
        string GetInsert();
    }
}