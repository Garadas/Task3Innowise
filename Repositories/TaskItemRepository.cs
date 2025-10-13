using Dapper;
using quest3.Factories;
using quest3.Models;
using System.Data;

namespace quest3.Repositories
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public TaskItemRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM TaskItems ORDER BY CreatedAt DESC";
            return await connection.QueryAsync<TaskItem>(sql);
        }

        public async Task<TaskItem> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "SELECT * FROM TaskItems WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<TaskItem>(sql, new { Id = id });
        }

        public async Task<int> CreateAsync(TaskItem taskitem)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                INSERT INTO TaskItems (Title, Description, IsCompleted, CreatedAt) 
                VALUES (@Title, @Description, @IsCompleted, @CreatedAt);
                SELECT last_insert_rowid();";

            return await connection.ExecuteScalarAsync<int>(sql, new
            {
                taskitem.Title,
                taskitem.Description,
                taskitem.IsCompleted,
                taskitem.CreatedAt
            });
        }

        public async Task<bool> UpdateAsync(TaskItem taskitem)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = @"
                UPDATE TaskItems 
                SET Title = @Title, Description = @Description, IsCompleted = @IsCompleted 
                WHERE Id = @Id";

            var affectedRows = await connection.ExecuteAsync(sql, taskitem);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "DELETE FROM TaskItems WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }

        public async Task<bool> ToggleCompletionStatusAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            const string sql = "UPDATE TaskItems SET IsCompleted = 1 - IsCompleted WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }
    }
}