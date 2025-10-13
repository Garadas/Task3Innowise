using System.Data;

namespace quest3.Factories
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}