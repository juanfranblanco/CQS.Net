using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Infrastructure.Repository.SqlGenerator;
using Infrastructure.ValueObjects;

namespace Infrastructure.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>, IGenericAsyncRepository<TEntity> where TEntity : new()
    {
        public GenericRepository(ISqlDeleteGenerator<TEntity> sqlDeleteGenerator, ISqlInsertGenerator<TEntity> sqlInsertGenerator, ISqlSelectGenerator<TEntity> sqlSelectGenerator, ISqlUpdateGenerator<TEntity> sqlUpdateGenerator, IEntityMetadata<TEntity> entityMetadata, IDbConnectionFactory dbConnectionFactory)
        {
            SqlDeleteGenerator = sqlDeleteGenerator;
            SqlInsertGenerator = sqlInsertGenerator;
            SqlSelectGenerator = sqlSelectGenerator;
            SqlUpdateGenerator = sqlUpdateGenerator;
            EntityMetadata = entityMetadata;
            DbConnectionFactory = dbConnectionFactory;
        }

        #region Properties

        protected ISqlDeleteGenerator<TEntity> SqlDeleteGenerator { get; set; }

        protected ISqlInsertGenerator<TEntity> SqlInsertGenerator { get; set; }

        protected ISqlSelectGenerator<TEntity> SqlSelectGenerator { get; set; }

        protected ISqlUpdateGenerator<TEntity> SqlUpdateGenerator { get; set; }

        protected IEntityMetadata<TEntity> EntityMetadata { get; set; } 

       
        protected IDbConnectionFactory DbConnectionFactory { get; private set; }

        #endregion

        #region Repository sync base actions

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {
            var sql = SqlSelectGenerator.GetSelectAll();
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();
                return connection.Query<TEntity>(sql);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetWhere(object filters)
        {
            var sql = SqlSelectGenerator.GetSelect(filters);
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();
                return connection.Query<TEntity>(sql, filters);
            }
        }

        public virtual IEnumerable<TEntity> GetWhere(object filters, int pageNumber, int pageSize)
        {
            var sql = SqlSelectGenerator.GetSelect(filters, pageSize, pageNumber);
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                return connection.Query<TEntity>(sql, filters);
            }
        }

        public PagedResult<TEntity> GetWherePagedResult(object filters, int pageNumber, int pageSize)
        {
            var total = GetCount(filters);
            var result = GetWhere(filters, pageNumber, pageSize);
            return new PagedResult<TEntity>(){PageNumber = pageNumber, PageSize = pageSize, Total = total, Result = result};
        }

        public virtual async Task<PagedResult<TEntity>> GetWherePagedResultAsync(object filters, int pageNumber, int pageSize)
        {
            var total = await GetCountAsync(filters);
            var result = await GetWhereAsync(filters, pageNumber, pageSize);
            return new PagedResult<TEntity>() { PageNumber = pageNumber, PageSize = pageSize, Total = total, Result = result };
        } 


        public virtual async Task<int> GetCountAsync(object filters)
        {
           
                var sql = SqlSelectGenerator.GetCount(filters);
                using (var connection = DbConnectionFactory.CreateConnection())
                {
                    connection.Open();
                    var result = await connection.QueryAsync<int>(sql, filters);

                    return result.Single();
                }
          
        }

        public virtual int GetCount(object filters)
        {
            var sql = SqlSelectGenerator.GetCount(filters);
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();
                return connection.Query<int>(sql, filters).Single();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public virtual TEntity GetFirst(object filters)
        {
            return this.GetWhere(filters).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public virtual bool Insert(TEntity instance)
        {
            bool added = false;
            var sql = SqlInsertGenerator.GetInsert();
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                if (EntityMetadata.IsIdentity)
                {
                    var newId = connection.Query<decimal>(sql, instance).Single();
                    added = newId > 0;

                    if (added)
                    {
                        var newParsedId = Convert.ChangeType(newId,
                            EntityMetadata.IdentityProperty.PropertyInfo.PropertyType);
                        EntityMetadata.IdentityProperty.PropertyInfo.SetValue(instance, newParsedId);
                    }
                }
                else
                {
                    added = connection.Execute(sql, instance) > 0;
                }
            }

            return added;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual bool Delete(object key)
        {
            var sql = SqlDeleteGenerator.GetDelete();
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();
                return connection.Execute(sql, key) > 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public virtual bool Update(TEntity instance)
        {
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();
                var sql = SqlUpdateGenerator.GetUpdate();
                return connection.Execute(sql, instance) > 0;
            }
        }

        #endregion

        #region Repository async base action

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            using (var connection = DbConnectionFactory.CreateConnection())
            {

                connection.Open();
                var sql = SqlSelectGenerator.GetSelectAll();
                return await connection.QueryAsync<TEntity>(sql, null);
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> GetWhereAsync(object filters) 
        {

            var sql = SqlSelectGenerator.GetSelect(filters);
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();
                return await connection.QueryAsync<TEntity>(sql, filters);
            }
        }


        public virtual async Task<IEnumerable<TEntity>> GetWhereAsync(object filters, int pageNumber, int pageSize)
        {
            var sql = SqlSelectGenerator.GetSelect(filters, pageSize, pageNumber);
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();
                return await connection.QueryAsync<TEntity>(sql, filters);
            }
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetFirstAsync(object filters)
        {
            var sql = SqlSelectGenerator.GetSelect(filters);
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();
                Task<IEnumerable<TEntity>> queryTask = connection.QueryAsync<TEntity>(sql, filters);
                IEnumerable<TEntity> data = await queryTask;
                return data.FirstOrDefault();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public virtual async Task<bool> InsertAsync(TEntity instance)
        {
            bool added = false;
            var sql = SqlInsertGenerator.GetInsert();

            using (var connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();
                if (EntityMetadata.IsIdentity)
                {
                    
                    Task<IEnumerable<decimal>> queryTask = connection.QueryAsync<decimal>(sql, instance);
                    IEnumerable<decimal> result = await queryTask;
                    var newId = result.Single();
                    added = newId > 0;

                    if (added)
                    {
                        var newParsedId = Convert.ChangeType(newId,
                            EntityMetadata.IdentityProperty.PropertyInfo.PropertyType);
                        EntityMetadata.IdentityProperty.PropertyInfo.SetValue(instance, newParsedId);
                    }
                }
                else
                {
                    await connection.QueryAsync<int>(sql, instance);
                    return true;
                }
            }
            return added;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(object key)
        {
            var sql = SqlDeleteGenerator.GetDelete();
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();
                await connection.QueryAsync<int>(sql, key);
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(TEntity instance)
        {
            using (var connection = DbConnectionFactory.CreateConnection())
            {
                connection.Open();
                var sql = SqlUpdateGenerator.GetUpdate();
                await connection.QueryAsync<int>(sql, instance);
                return true;
            }
        }

        #endregion
    }
}