using Dapper;
using quest3.Factories;
using System.Data;

namespace quest3.Database
{
    public class DatabaseInitializer
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DatabaseInitializer(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public void Initialize()
        {
            using var connection = _connectionFactory.CreateConnection();

            const string sql = @"
                CREATE TABLE IF NOT EXISTS TaskItems (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Title NVARCHAR(255) NOT NULL,
                    Description NVARCHAR(1000),
                    IsCompleted INTEGER NOT NULL DEFAULT 0,
                    CreatedAt DATETIME NOT NULL
                )";

            connection.Execute(sql);
        }
    }
}