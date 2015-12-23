using System;
using System.Linq;
using System.Text;

namespace Infrastructure.Repository.SqlGenerator
{
    public class SqlUpdateGenerator<TEntity> : ISqlUpdateGenerator<TEntity> where TEntity : new()
    {
        protected IEntityMetadata<TEntity> EntityMetadata { get; private set; }

        public SqlUpdateGenerator(IEntityMetadata<TEntity> entityMetadata)
        {
            EntityMetadata = entityMetadata;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetUpdate()
        {
            var properties = EntityMetadata.BaseProperties.Where(p => !EntityMetadata.KeyProperties.Any(k => k.Name.Equals(p.Name, StringComparison.InvariantCultureIgnoreCase)));

            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("UPDATE [{0}] SET {1} WHERE {2}",
                EntityMetadata.TableName,
                string.Join(", ", properties.Select(p => string.Format("[{0}].[{1}] = @{2}", EntityMetadata.TableName, p.ColumnName, p.Name))),
                string.Join(" AND ", EntityMetadata.KeyProperties.Select(p => string.Format("[{0}].[{1}] = @{2}", EntityMetadata.TableName, p.ColumnName, p.Name))));

            return sqlBuilder.ToString();
        }

    }
}