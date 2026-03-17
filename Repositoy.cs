using System.Text.Json;

public interface ITaskRepository
{
    MyArray<TaskItem> LoadTasks();
    void SaveTasks(MyArray<TaskItem> tasks);
}

public class JsonTaskRepository : ITaskRepository
{
    private readonly string _filePath;

    public JsonTaskRepository(string filePath) => _filePath = filePath;

    public MyArray<TaskItem> LoadTasks()
    {
        if (!File.Exists(_filePath))
        {
            return new MyArray<TaskItem>();
        }

        string json = File.ReadAllText(_filePath);
        var tasks = JsonSerializer.Deserialize<TaskItem[]>(json);
        return tasks is null ? new MyArray<TaskItem>() : new MyArray<TaskItem>(tasks);
    }

    public void SaveTasks(MyArray<TaskItem> tasks)
    {
        string json = JsonSerializer.Serialize(tasks.ToArray(), new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }
}