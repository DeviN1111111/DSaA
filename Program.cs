class Program
{
    static void Main(string[] args)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "tasks.json");
        ITaskRepository repository = new JsonTaskRepository(filePath);
        ITaskService service = new TaskService(repository);
        ITaskView view = new ConsoleTaskView(service);

        view.Run();
    }
}