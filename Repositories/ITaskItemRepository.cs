using quest3.Models;

namespace quest3.Repositories
{
    public interface ITaskItemRepository : IRepository<TaskItem>
    {
        Task<bool> ToggleCompletionStatusAsync(int id);
    }
}