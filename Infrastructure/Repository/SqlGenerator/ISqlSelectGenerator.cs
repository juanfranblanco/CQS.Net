namespace Infrastructure.Repository.SqlGenerator
{
    public interface ISqlSelectGenerator<TEntity> where TEntity : new()
    {
        string GetSelectAll();
        string GetSelect(object filters);
        string GetSelect(object filters, int pageSize, int pageNumber);
        string GetCount(object filters);
    }
}