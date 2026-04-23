using System.Collections.Generic;
using System.Linq;

public interface ITaskService
{
    IEnumerable<TaskItem> GetAllTasks();
    void AddTask(string description);
    void RemoveTask(int id);
    void ToggleTaskCompletion(int id);
    void ChangeTaskPriority(int id, string priority);
    void ChangeTaskStatus(int id, string status);
    void ChangeTaskAssignees(int id, string name, bool add);
    void ChangeTaskPrevious(int id, int? previousTask);
}

public class TaskService : ITaskService 
{
    private readonly ITaskRepository _repository;
    private readonly IMyCollection<TaskItem> _tasks;

    public TaskService(ITaskRepository repository) 
    {
        _repository = repository;
        _tasks = _repository.LoadTasks();
    }

    public IEnumerable<TaskItem> GetAllTasks() => _tasks.ToArray();

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

        var newTask = new TaskItem { Id = newId, Description = description, Completed = false};

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
            string[] newAssignees = new string[task.Assignees.Length + 1];
            for (int i = 0; i < task.Assignees.Length; i++)
            {
                newAssignees[i] = task.Assignees[i];
            }
            newAssignees[newAssignees.Length - 1] = name;
            task.Assignees = newAssignees;
            _repository.SaveTasks(_tasks);
        }
        else if (task != null && add == false)
        {
            string[] newAssignees = new string[task.Assignees.Length - 1];
            int index = 0;
            for (int i = 0; i < task.Assignees.Length; i++)
            {
                if (task.Assignees[i] != name)
                {
                    newAssignees[index++] = task.Assignees[i];
                }
            }
            task.Assignees = newAssignees;
            _repository.SaveTasks(_tasks);
        }
    }
    
    public void ChangeTaskPrevious(int id, int? previousTaskId)
    {
        var task = _tasks.FindBy(id, (t, key) => t.Id == key);

        if (task != null && previousTaskId.HasValue)
        {
            bool exists = false;

            for (int i = 0; i < task.Previous.Length; i++)
            {
                if (task.Previous[i] == previousTaskId.Value)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                int[] newPrevious = new int[task.Previous.Length + 1];

                for (int i = 0; i < task.Previous.Length; i++)
                {
                    newPrevious[i] = task.Previous[i];
                }

                newPrevious[newPrevious.Length - 1] = previousTaskId.Value;
                task.Previous = newPrevious;

                _repository.SaveTasks(_tasks);
            }
        }
        else if (task != null && !previousTaskId.HasValue)
        {
            task.Previous = new int[0];
            _repository.SaveTasks(_tasks);
        }
    }
}