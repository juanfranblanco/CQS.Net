namespace Infrastructure.Repository.SqlGenerator
{
    public interface ISqlDeleteGenerator<TEntity>  where TEntity : new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetDelete();
    }
}