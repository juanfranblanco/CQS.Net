using System;
using System.Linq;
using System.Text;

namespace Infrastructure.Repository.SqlGenerator
{
    public class SqlSelectGenerator<TEntity> : ISqlSelectGenerator<TEntity> where TEntity : new()
    {
        protected IEntityMetadata<TEntity> EntityMetadata { get; private set; }

        protected ISqlWhereGenerator<TEntity> SqlWhereGenerator { get; private set; }

        public SqlSelectGenerator(IEntityMetadata<TEntity> entityMetadata, ISqlWhereGenerator<TEntity> sqlWhereGenerator)
        {
            EntityMetadata = entityMetadata;
            SqlWhereGenerator = sqlWhereGenerator;
        }

        public virtual string GetSelectAll()
        {
            return this.GetSelect(new { });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public virtual string GetSelect(object filters)
        {
            //Projection function
            Func<PropertyMetadata, string> projectionFunction = (p) =>
            {
                if (!string.IsNullOrEmpty(p.Alias))
                    return string.Format("[{0}].[{1}] AS [{2}]", EntityMetadata.TableName, p.ColumnName, p.Name);

                return string.Format("[{0}].[{1}]", EntityMetadata.TableName, p.ColumnName);
            };

            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT {0} FROM [{1}] WITH (NOLOCK)",
                string.Join(", ", EntityMetadata.BaseProperties.Select(projectionFunction)),
                EntityMetadata.TableName);

            sqlBuilder.Append(SqlWhereGenerator.GetWhere(filters));

            return sqlBuilder.ToString();
        }

        /*
         * 
         * Suffix  for SqlServer2012
          ORDER BY SalesOrderDetailID
          OFFSET (@PageNumber-1)*@RowsPerPage ROWS
          FETCH NEXT @RowsPerPage ROWS ONLY
         */

        public string GetSelect(object filters, int pageSize, int pageNumber)
        {
            var select = this.GetSelect(filters);
            var pagedSuffix =
                @"
            ORDER BY {0}
            OFFSET ({1}-1)*{2} ROWS
            FETCH NEXT {2} ROWS ONLY ";

            return select + String.Format(pagedSuffix, EntityMetadata.IdentityProperty.ColumnName, pageNumber, pageSize);
        }

        public string GetCount(object filters)
        {
            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("SELECT COUNT(*) FROM [{0}] WITH (NOLOCK)",
                EntityMetadata.TableName);
            sqlBuilder.Append(SqlWhereGenerator.GetWhere(filters));
            return sqlBuilder.ToString();
        }

    }
}