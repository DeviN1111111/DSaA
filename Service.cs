using System.Collections.Generic;

public interface ITaskService
{
    IEnumerable<TaskItem> GetAllTasks();
    void AddTask(string description);
    void RemoveTask(int id);
    void ToggleTaskCompletion(int id);
    void ChangeTaskPriority(int id, string priority);
    void ChangeTaskStatus(int id, string status);
    void ChangeTaskAssignees(int id, string name, bool add);
}

public class TaskService : ITaskService 
{
    private readonly ITaskRepository _repository;
    private readonly MyArray<TaskItem> _tasks;

    public TaskService(ITaskRepository repository) 
    {
        _repository = repository;
        _tasks = _repository.LoadTasks();
    }

    public IEnumerable<TaskItem> GetAllTasks() => _tasks;

    public void AddTask(string description) 
    {
        // int newId = _tasks.Count > 0 ? _tasks[newArray.Count - 1].Id + 1 : 1;
        var arr = _tasks.ToArray();
        int newId = 1;

        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] != null && arr[i].Id >= newId)
                newId = arr[i].Id + 1;
        }

        var newTask = new TaskItem { Id = newId, Description = description, Completed = false };

        _tasks.Add(newTask);
        _repository.SaveTasks(_tasks);
    }

    public void RemoveTask(int id) 
    {
        var task = _tasks.FindBy(id, (t, key) => t.Id == key);
        if (task != null) {
            _tasks.Remove(task);
            _repository.SaveTasks(_tasks);
        }
    }

    public void ToggleTaskCompletion(int id) 
    {
        var task = _tasks.FindBy(id, (t, key) => t.Id == key);
        if (task != null) {
            task.Completed = !task.Completed;
            _repository.SaveTasks(_tasks);
        }
    }

    public void ChangeTaskPriority(int id, string priority)
    {
        var task = _tasks.FindBy(id, (t, key) => t.Id == key);
        if (task != null) {
            task.Priority = priority;
            _repository.SaveTasks(_tasks);
        }
    }

    public void ChangeTaskStatus(int id, string status)
    {
        var task = _tasks.FindBy(id, (t, key) => t.Id == key);
        if (task != null) {
            task.Status = status;
            _repository.SaveTasks(_tasks);
        }
    }

    public void ChangeTaskAssignees(int id, string name, bool add = false)
    {
        var task = _tasks.FindBy(id, (t, key) => t.Id == key);
        if (task != null && add == true) 
        {
            task.Assignees.Add(name);
            _repository.SaveTasks(_tasks);
        }
        else
        {
            task!.Assignees.Remove(name);
            _repository.SaveTasks(_tasks);
        }
    }
}