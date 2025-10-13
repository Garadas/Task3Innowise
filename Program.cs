using quest3.Database;
using quest3.Factories;
using quest3.Repositories;
using quest3.Services;
using quest3.UI;

class Program
{
    static async Task Main(string[] args)
    {
        // Правильный connection string для SQLite
        var connectionFactory = new SqliteConnectionFactory("Data Source=taskitems.db");

        var databaseInitializer = new DatabaseInitializer(connectionFactory);
        var taskitemRepository = new TaskItemRepository(connectionFactory);
        var taskitemService = new TaskItemService(taskitemRepository);
        var consoleMenu = new ConsoleMenu(taskitemService);

        // Инициализация базы данных
        databaseInitializer.Initialize();

        // Запуск приложения
        await consoleMenu.RunAsync();
    }
}