using quest3.Models;
using quest3.Repositories;

namespace quest3.Services
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskitemRepository;

        public TaskItemService(ITaskItemRepository taskitemRepository)
        {
            _taskitemRepository = taskitemRepository;
        }

        public async Task<IEnumerable<TaskItem>> GetAllTaskItemsAsync()
        {
            return await _taskitemRepository.GetAllAsync();
        }

        public async Task<TaskItem> GetTaskItemByIdAsync(int id)
        {
            return await _taskitemRepository.GetByIdAsync(id);
        }

        public async Task<int> CreateTaskItemAsync(string title, string description)
        {
            var taskitem = new TaskItem
            {
                Title = title,
                Description = description,
                IsCompleted = false,
                CreatedAt = DateTime.Now
            };

            return await _taskitemRepository.CreateAsync(taskitem);
        }

        public async Task<bool> UpdateTaskItemAsync(TaskItem taskitem)
        {
            return await _taskitemRepository.UpdateAsync(taskitem);
        }

        public async Task<bool> DeleteTaskItemAsync(int id)
        {
            return await _taskitemRepository.DeleteAsync(id);
        }

        public async Task<bool> ToggleTaskItemCompletionAsync(int id)
        {
            return await _taskitemRepository.ToggleCompletionStatusAsync(id);
        }
    }
}