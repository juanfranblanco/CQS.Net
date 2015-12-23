using System.Linq;
using System.Text;

namespace Infrastructure.Repository.SqlGenerator
{
    public class SqlDeleteGenerator<TEntity> : ISqlDeleteGenerator<TEntity> where TEntity : new()
    {
        protected IEntityMetadata<TEntity> EntityMetadata { get; private set; }

        public SqlDeleteGenerator(IEntityMetadata<TEntity> entityMetadata)
        {
            EntityMetadata = entityMetadata;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetDelete()
        {
            var sqlBuilder = new StringBuilder();

            if (!EntityMetadata.LogicalDelete)
            {
                sqlBuilder.AppendFormat("DELETE FROM [{0}] WHERE {1}",
                    EntityMetadata.TableName,
                    string.Join(" AND ", EntityMetadata.KeyProperties.Select(p => string.Format("[{0}].[{1}] = @{2}", EntityMetadata.TableName, p.ColumnName, p.Name))));

            }
            else
                sqlBuilder.AppendFormat("UPDATE [{0}] SET {1} WHERE {2}",
                    EntityMetadata.TableName,
                    string.Format("[{0}].[{1}] = {2}", EntityMetadata.TableName, EntityMetadata.StatusProperty.ColumnName, EntityMetadata.LogicalDeleteValue),
                    string.Join(" AND ", EntityMetadata.KeyProperties.Select(p => string.Format("[{0}].[{1}] = @{2}", EntityMetadata.TableName, p.ColumnName, p.Name))));


            return sqlBuilder.ToString();
        }  

    }
}