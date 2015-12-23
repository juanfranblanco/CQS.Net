namespace Infrastructure.Repository.SqlGenerator
{
    public interface ISqlUpdateGenerator<TEntity>  where TEntity : new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        string GetUpdate();
    }
}