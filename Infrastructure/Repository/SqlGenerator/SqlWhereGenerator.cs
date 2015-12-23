using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Repository.SqlGenerator
{
    public class SqlWhereGenerator<TEntity> : ISqlWhereGenerator<TEntity> where TEntity : new()
    {
        protected IEntityMetadata<TEntity> EntityMetadata { get; private set; }

        public SqlWhereGenerator(IEntityMetadata<TEntity> entityMetadata)
        {
            EntityMetadata = entityMetadata;
        }


        public virtual string ToWhere(IEnumerable<string> properties)
        {
            return string.Join(" AND ", properties.Select(p =>
            {

                var propertyMetadata =
                    EntityMetadata.BaseProperties.FirstOrDefault(
                        pm => pm.Name.Equals(p, StringComparison.InvariantCultureIgnoreCase));

                if (propertyMetadata != null)
                    return string.Format("[{0}].[{1}] = @{2}", EntityMetadata.TableName, propertyMetadata.ColumnName,
                        propertyMetadata.Name);

                return string.Format("[{0}].[{1}] = @{2}", EntityMetadata.TableName, p, p);

            }));
        }

        public virtual string GetWhere(object filters)
        {
            var sqlBuilder = new StringBuilder();
            //Properties of the dynamic filters object
            var filterProperties = filters.GetType().GetProperties().Select(p => p.Name);
            bool containsFilter = (filterProperties != null && filterProperties.Any());

            if (containsFilter)
                sqlBuilder.AppendFormat(" WHERE {0} ", this.ToWhere(filterProperties));

            //Evaluates if EntityMetadata repository implements logical delete
            if (EntityMetadata.LogicalDelete)
            {
                if (containsFilter)
                    sqlBuilder.AppendFormat(" AND [{0}].[{1}] != {2}",
                        EntityMetadata.TableName,
                        EntityMetadata.StatusProperty.Name,
                        EntityMetadata.LogicalDeleteValue);
                else
                    sqlBuilder.AppendFormat(" WHERE [{0}].[{1}] != {2}",
                        EntityMetadata.TableName,
                        EntityMetadata.StatusProperty.Name,
                        EntityMetadata.LogicalDeleteValue);
            }
            return sqlBuilder.ToString();

        }

    }
}