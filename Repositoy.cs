using System.Text.Json;

public interface ITaskRepository
{
    IMyCollection<TaskItem> LoadTasks();
    void SaveTasks(IMyCollection<TaskItem> tasks);
}

public class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;

    public JsonTaskRepository(string filePath) => _filePath = filePath;

    public IMyCollection<TaskItem> LoadTasks()
    {
        if (!File.Exists(_filePath))
        {
            return new MyArray<TaskItem>();
        }

        string json = File.ReadAllText(_filePath);
        var tasks = JsonSerializer.Deserialize<TaskItem[]>(json);
        return tasks is null ? new MyArray<TaskItem>() : new MyArray<TaskItem>(tasks);
    }

    public void SaveTasks(IMyCollection<TaskItem> tasks)
    {
        string json = JsonSerializer.Serialize(tasks.ToArray(), new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}