
using System.Data;

namespace Infrastructure.Repository
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}

