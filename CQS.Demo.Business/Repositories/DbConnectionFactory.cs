using System.Data;
using System.Data.SqlClient;
using Infrastructure.Repository;

namespace CQS.Demo.Business.Repositories
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(@"Server=.\SQLEXPRESS;Database=Store;Trusted_Connection=True;");
        }
    }
}