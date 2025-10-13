using quest3.Models;

namespace quest3.Services
{
    public interface ITaskItemService
    {
        Task<IEnumerable<TaskItem>> GetAllTaskItemsAsync();
        Task<TaskItem> GetTaskItemByIdAsync(int id);
        Task<int> CreateTaskItemAsync(string title, string description);
        Task<bool> UpdateTaskItemAsync(TaskItem taskitem);
        Task<bool> DeleteTaskItemAsync(int id);
        Task<bool> ToggleTaskItemCompletionAsync(int id);
    }
}