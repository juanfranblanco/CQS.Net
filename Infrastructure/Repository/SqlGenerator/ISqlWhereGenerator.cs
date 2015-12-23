namespace Infrastructure.Repository.SqlGenerator
{
    public interface ISqlWhereGenerator<TEntity>  where TEntity : new()
    {
        string GetWhere(object filters);
    }
}