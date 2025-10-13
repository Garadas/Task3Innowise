using quest3.Models;
using quest3.Services;

namespace quest3.UI
{
    public class ConsoleMenu
    {
        private readonly ITaskItemService _taskitemService;

        public ConsoleMenu(ITaskItemService taskitemService)
        {
            _taskitemService = taskitemService;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("=== Task Manager ===");

            while (true)
            {
                DisplayMainMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": await ShowAllTaskItemsAsync(); break;
                    case "2": await CreateNewTaskItemAsync(); break;
                    case "3": await GetTaskItemByIdAsync(); break;
                    case "4": await UpdateTaskItemAsync(); break;
                    case "5": await DeleteTaskItemAsync(); break;
                    case "6": await ToggleTaskItemCompletionAsync(); break;
                    case "7": return;
                    default: ShowError("Неверный выбор! Попробуйте снова."); break;
                }
            }
        }

        private void DisplayMainMenu()
        {
            Console.WriteLine("\n=== ГЛАВНОЕ МЕНЮ ===");
            Console.WriteLine("1. Просмотреть все задачи");
            Console.WriteLine("2. Добавить новую задачу");
            Console.WriteLine("3. Найти задачу по ID");
            Console.WriteLine("4. Обновить задачу");
            Console.WriteLine("5. Удалить задачу");
            Console.WriteLine("6. Изменить статус выполнения");
            Console.WriteLine("7. Выход");
            Console.Write("Выберите действие: ");
        }

        private async Task ShowAllTaskItemsAsync()
        {
            Console.WriteLine("\n=== ВСЕ ЗАДАЧИ ===");

            var taskitems = await _taskitemService.GetAllTaskItemsAsync();

            if (!taskitems.Any())
            {
                ShowInfo("Задачи не найдены.");
                return;
            }

            foreach (var taskitem in taskitems)
            {
                DisplayTaskItem(taskitem);
            }
        }

        private async Task CreateNewTaskItemAsync()
        {
            Console.WriteLine("\n=== СОЗДАНИЕ НОВОЙ ЗАДАЧИ ===");

            var title = GetInput("Введите заголовок задачи: ", required: true);
            if (string.IsNullOrEmpty(title)) return;

            var description = GetInput("Введите описание задачи: ");

            try
            {
                var taskitemId = await _taskitemService.CreateTaskItemAsync(title, description);
                ShowSuccess($"Задача успешно создана с ID: {taskitemId}");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при создании задачи: {ex.Message}");
            }
        }

        private async Task GetTaskItemByIdAsync()
        {
            Console.WriteLine("\n=== ПОИСК ЗАДАЧИ ПО ID ===");

            if (!TryGetId(out int id)) return;

            try
            {
                var taskitem = await _taskitemService.GetTaskItemByIdAsync(id);
                if (taskitem != null)
                {
                    DisplayTaskItemDetails(taskitem);
                }
                else
                {
                    ShowError("Задача с указанным ID не найдена.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при поиске задачи: {ex.Message}");
            }
        }

        private async Task UpdateTaskItemAsync()
        {
            Console.WriteLine("\n=== ОБНОВЛЕНИЕ ЗАДАЧИ ===");

            if (!TryGetId(out int id)) return;

            try
            {
                var taskitem = await _taskitemService.GetTaskItemByIdAsync(id);
                if (taskitem == null)
                {
                    ShowError("Задача с указанным ID не найдена.");
                    return;
                }

                taskitem.Title = GetInput($"Текущий заголовок: {taskitem.Title}\nНовый заголовок: ", taskitem.Title);
                taskitem.Description = GetInput($"Текущее описание: {taskitem.Description}\nНовое описание: ", taskitem.Description);

                var success = await _taskitemService.UpdateTaskItemAsync(taskitem);
                ShowResult(success, "Задача успешно обновлена!", "Ошибка при обновлении задачи.");
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при обновлении задачи: {ex.Message}");
            }
        }

        private async Task DeleteTaskItemAsync()
        {
            Console.WriteLine("\n=== УДАЛЕНИЕ ЗАДАЧИ ===");

            if (!TryGetId(out int id)) return;

            try
            {
                var taskitem = await _taskitemService.GetTaskItemByIdAsync(id);
                if (taskitem == null)
                {
                    ShowError("Задача с указанным ID не найдена.");
                    return;
                }

                Console.WriteLine("Вы действительно хотите удалить задачу?");
                DisplayTaskItemDetails(taskitem);
                Console.Write("Подтвердите удаление (y/n): ");

                if (ConfirmAction())
                {
                    var success = await _taskitemService.DeleteTaskItemAsync(id);
                    ShowResult(success, "Задача успешно удалена!", "Ошибка при удалении задачи.");
                }
                else
                {
                    ShowInfo("Удаление отменено.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при удалении задачи: {ex.Message}");
            }
        }

        private async Task ToggleTaskItemCompletionAsync()
        {
            Console.WriteLine("\n=== ИЗМЕНЕНИЕ СТАТУСА ВЫПОЛНЕНИЯ ===");

            if (!TryGetId(out int id)) return;

            try
            {
                var taskitem = await _taskitemService.GetTaskItemByIdAsync(id);
                if (taskitem == null)
                {
                    ShowError("Задача с указанным ID не найдена.");
                    return;
                }

                var oldStatus = taskitem.IsCompleted ? "Выполнена" : "Не выполнена";
                var newStatus = !taskitem.IsCompleted ? "Выполнена" : "Не выполнена";

                Console.WriteLine($"Текущий статус: {oldStatus}");
                Console.WriteLine($"Новый статус: {newStatus}");
                Console.Write("Подтвердить изменение (y/n): ");

                if (ConfirmAction())
                {
                    var success = await _taskitemService.ToggleTaskItemCompletionAsync(id);
                    ShowResult(success, "Статус задачи успешно изменен!", "Ошибка при изменении статуса.");
                }
                else
                {
                    ShowInfo("Изменение статуса отменено.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Ошибка при изменении статуса: {ex.Message}");
            }
        }
        private void DisplayTaskItem(TaskItem taskitem)
        {
            var status = taskitem.IsCompleted ? "[complete]" : "[ ]";
            Console.WriteLine($"ID: {taskitem.Id}");
            Console.WriteLine($"  Заголовок: {taskitem.Title}");
            Console.WriteLine($"  Описание: {taskitem.Description}");
            Console.WriteLine($"  Статус: {status}");
            Console.WriteLine($"  Создано: {taskitem.CreatedAt:dd.MM.yyyy HH:mm}");
            Console.WriteLine("  " + new string('-', 30));
        }

        private void DisplayTaskItemDetails(TaskItem taskitem)
        {
            var status = taskitem.IsCompleted ? "Выполнена" : "Не выполнена";
            Console.WriteLine("Задача найдена:");
            Console.WriteLine($"   ID: {taskitem.Id}");
            Console.WriteLine($"   Заголовок: {taskitem.Title}");
            Console.WriteLine($"   Описание: {taskitem.Description}");
            Console.WriteLine($"   Статус: {status}");
            Console.WriteLine($"   Создано: {taskitem.CreatedAt:dd.MM.yyyy HH:mm}");
        }

        private string GetInput(string prompt, string defaultValue = "", bool required = false)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                if (required)
                {
                    ShowError("Это поле обязательно для заполнения!");
                    return string.Empty;
                }
                return defaultValue;
            }

            return input;
        }

        private bool TryGetId(out int id)
        {
            Console.Write("Введите ID задачи: ");
            if (int.TryParse(Console.ReadLine(), out id))
                return true;

            ShowError("Неверный формат ID!");
            return false;
        }

        private bool ConfirmAction()
        {
            var confirmation = Console.ReadLine()?.ToLower();
            return confirmation == "y" || confirmation == "yes" || confirmation == "д";
        }

        private void ShowSuccess(string message) => Console.WriteLine(message);
        private void ShowError(string message) => Console.WriteLine(message);
        private void ShowInfo(string message) => Console.WriteLine(message);

        private void ShowResult(bool success, string successMessage, string errorMessage)
        {
            if (success) ShowSuccess(successMessage);
            else ShowError(errorMessage);
        }
    }
}