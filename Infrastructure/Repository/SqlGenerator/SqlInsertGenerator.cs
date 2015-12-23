using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Repository.SqlGenerator
{
    public class SqlInsertGenerator<TEntity> : ISqlInsertGenerator<TEntity> where TEntity : new()
    {
        protected IEntityMetadata<TEntity> EntityMetadata { get; private set; }

        public SqlInsertGenerator(IEntityMetadata<TEntity> entityMetadata)
        {
            EntityMetadata = entityMetadata;
        }

        public virtual string GetInsert()
        {
            //Enumerate the entity properties
            //Identity property (if exists) has to be ignored
            IEnumerable<PropertyMetadata> properties = (EntityMetadata.IsIdentity ?
                EntityMetadata.BaseProperties.Where(p => !p.Name.Equals(EntityMetadata.IdentityProperty.Name, StringComparison.InvariantCultureIgnoreCase)) :
                EntityMetadata.BaseProperties).ToList();

            string columNames = string.Join(", ", properties.Select(p => string.Format("[{0}].[{1}]", EntityMetadata.TableName, p.ColumnName)));
            string values = string.Join(", ", properties.Select(p => string.Format("@{0}", p.Name)));

            var sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("INSERT INTO [{0}] {1} {2} ",
                EntityMetadata.TableName,
                string.IsNullOrEmpty(columNames) ? string.Empty : string.Format("({0})", columNames),
                string.IsNullOrEmpty(values) ? string.Empty : string.Format(" VALUES ({0})", values));

            //If the entity has an identity key, we create a new variable into the query in order to retrieve the generated id
            if (EntityMetadata.IsIdentity)
            {
                sqlBuilder.AppendLine("DECLARE @NEWID NUMERIC(38, 0)");
                sqlBuilder.AppendLine("SET	@NEWID = SCOPE_IDENTITY()");
                sqlBuilder.AppendLine("SELECT @NEWID");
            }

            return sqlBuilder.ToString();
        }
    }


}