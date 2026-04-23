using System.Runtime.CompilerServices;
using Spectre.Console;

interface ITaskView
{
    void Run();
}

public class ConsoleTaskView : ITaskView 
{
    private readonly ITaskService _service;

    public ConsoleTaskView(ITaskService service) 
    {
        _service = service;
    }

    void DisplayTasks(IEnumerable<TaskItem> tasks) 
    {
        Console.Clear();
        Console.WriteLine("==== ToDo List ====");
        foreach (var task in tasks)
            Console.WriteLine($"{task}");
    }

    string Prompt(string prompt) 
    {
        Console.Write(prompt);
        return Console.ReadLine()!;
    }

    string SelectUser()
    {
        var selecteduser = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Select User")
            .AddChoices("Cheng", "Devin", "Carlos"));  

        return selecteduser;      
    }

    public void Run() 
    {
        Console.Clear();
        IMyCollection<TaskItem> myCollection = new MyArray<TaskItem>();
        foreach (var task in _service.GetAllTasks())
        {
            myCollection.Add(task);
        }
        string currentDataType = "Array";
        bool filter = false;
        string filterString = "";
        string filterType = "";

        System.Console.WriteLine("\n==== Select User ====");
        var currentUser = SelectUser();

        while (true) 
        {
            Console.Clear();
            IMyCollection<TaskItem> myFilterCollection = myCollection;
            if(filter)
            {
                if(filterType == "Sort")
                {
                    myFilterCollection.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));
                }
                else if(filterType == "Priority")
                {
                    myFilterCollection = myFilterCollection.Filter((task) => task.Priority == filterString);
                }
                else if(filterType == "Status")
                {
                    myFilterCollection = myFilterCollection.Filter((task) => task.Status == filterString);
                }
            }
            LayoutBuilder<TaskItem>.RenderLayout(myFilterCollection.ToArray());

            Console.WriteLine("\n==== ToDo List ====");
            Console.WriteLine($"Current DataType: {currentDataType}");
            Console.WriteLine($"Current User: {currentUser}");
            Console.WriteLine("\nOptions:");
            Console.WriteLine("0. Change Datatype");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Remove Task");
            Console.WriteLine("3. Add or Remove Assignees");
            Console.WriteLine("4. Change Task Priority");
            Console.WriteLine("5. Change Task Status");
            Console.WriteLine("6. Toggle Filter");
            Console.WriteLine("7. Assign Previous Task");
            Console.WriteLine("8. See Workflow");
            Console.WriteLine("9. Exit");

            string option = Prompt("Select an option: ");
            switch (option) {
                case "0":
                    var selectedDataType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Select DataType")
                        .AddChoices("Array", "Linked List", "Hashmap"));

                    if(currentDataType == selectedDataType)
                    {
                        Console.WriteLine($"Already using {selectedDataType}");
                    }
                    else if(selectedDataType == "Array")
                    {
                        var currentItems = myCollection.ToArray();
                        myCollection = new MyArray<TaskItem>(currentItems);
                        currentDataType = "Array";
                        Console.WriteLine("Switched to MyArray");
                    }
                    else if(selectedDataType == "Linked List")
                    {
                        var currentItems = myCollection.ToArray();
                        myCollection = new MyLinkedList<TaskItem>(currentItems);
                        currentDataType = "Linked List";
                        Console.WriteLine("Switched to MyLinkedList");
                    }
                    else if(selectedDataType == "Hashmap")
                    {
                        var currentItems = myCollection.ToArray();
                        myCollection = (IMyCollection<TaskItem>)new MyHashMap<int, TaskItem>(
                            item => item.Id,
                            currentItems
                        );
                        currentDataType = "Hashmap";
                        Console.WriteLine("Switched to MyHashMap");
                    }
                    Console.ReadKey();
                    break;
                case "1":
                    string description = Prompt("Enter task description: ");
                    string priority = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Choose your priority")
                        .AddChoices("Low", "Middle", "High"));

                    var arr = myCollection.ToArray();
                    int newId = 1;

                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i] != null && arr[i].Id >= newId)
                            newId = arr[i].Id + 1;
                    }

                    TaskItem newTask = new TaskItem { Id = newId, Description = description, Completed = false, Priority = priority};
                    myCollection.Add(newTask);
                    _service.AddTask(description);
                    _service.ChangeTaskPriority(newId, priority);
                    break;
                case "2":
                    string removeIdStr = Prompt("Enter task id to remove: ");
                    if (int.TryParse(removeIdStr, out int removeId)) 
                    {
                        TaskItem ItemToRemove = myCollection.FindBy<int>(removeId, (item, Id) => item.Id == Id);
                        
                        if(ItemToRemove != default)
                        {
                            myCollection.Remove(ItemToRemove);
                            _service.RemoveTask(removeId);
                            System.Console.WriteLine($"ID: {removeId} has been deleted");
                        }
                        else
                        {
                            System.Console.WriteLine("No task with given ID");
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Invalid ID format");
                    }
                    Console.ReadKey();
                    break;
                case "3":
                    string choice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Add or Remove")
                        .AddChoices("Add", "Remove"));
                    string taskId = Prompt("Enter task id to add or remove assignees: ");

                    if (int.TryParse(taskId, out int taskID)) 
                    {
                        if(choice == "Add")
                        {
                            TaskItem ItemToAddAssignees = myCollection.FindBy<int>(taskID, (item, Id) => item.Id == Id);
                            if(ItemToAddAssignees == null)
                            {
                                System.Console.WriteLine("Doesn't exist!");
                                Console.ReadKey();
                                break;
                            }
                            string name = SelectUser();

                            bool HasSameAssignee = false;
                            foreach(var assignee in ItemToAddAssignees.Assignees)
                            {
                                if(assignee == name)
                                {
                                    HasSameAssignee = true ;
                                }
                            }

                            if (!HasSameAssignee)
                            {
                                string[] newAssignees = new string[ItemToAddAssignees.Assignees.Length + 1];
                                for (int i = 0; i < ItemToAddAssignees.Assignees.Length; i++)
                                {
                                    newAssignees[i] = ItemToAddAssignees.Assignees[i];
                                }
                                newAssignees[ItemToAddAssignees.Assignees.Length] = name;
                                ItemToAddAssignees.Assignees = newAssignees;

                                _service.ChangeTaskAssignees(taskID, name, true);
                            }

                            if (HasSameAssignee) System.Console.WriteLine("Member already assigned");
                            Console.ReadLine();
                        }
                        else
                        {
                            TaskItem item = myCollection.FindBy<int>(taskID, (x, id) => x.Id == id);
                            if (item == null)
                            {
                                Console.WriteLine("Doesn't exist!");
                                Console.ReadKey();
                                break;
                            }

                            string name = SelectUser();

                            int index = -1;
                            for (int i = 0; i < item.Assignees.Length; i++)
                            {
                                if (item.Assignees[i] == name)
                                {
                                    index = i;
                                    break;
                                }
                            }

                            if (index != -1)
                            {
                                string[] newAssignees = new string[item.Assignees.Length - 1];
                                int j = 0;

                                for (int i = 0; i < item.Assignees.Length; i++)
                                {
                                    if (i != index)
                                    {
                                        newAssignees[j] = item.Assignees[i];
                                        j++;
                                    }
                                }

                                item.Assignees = newAssignees;
                                _service.ChangeTaskAssignees(taskID, name, false);
                            }
                            else
                            {
                                Console.WriteLine($"Assignee '{name}' not found.");
                                Console.ReadKey();
                            }
                        }
                    }
                    break;
                case "4":  
                    string priorityChange = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Choose your priority")
                        .AddChoices("Low", "Middle", "High"));

                    System.Console.WriteLine("Enter ID to change: ");
                    if (int.TryParse(Console.ReadLine(), out int changeIdStr))
                    {
                        var array = myCollection;
                        
                        foreach(var item in array)
                        {
                            if (item.Id == changeIdStr)
                            {
                                bool HasSameAssignee3 = false;
                                foreach(var assignee in item.Assignees)
                                {
                                    if(assignee == currentUser)
                                    {
                                        HasSameAssignee3 = true;
                                    }
                                }
                                if (HasSameAssignee3 || item.Assignees.Length == 0)
                                {
                                    _service.ChangeTaskPriority(changeIdStr, priorityChange);
                                    TaskItem taskToChange = myCollection.FindBy<int>(changeIdStr, (taskItem, id) => taskItem.Id == id);
                                    taskToChange.Priority = priorityChange;
                                }
                                else
                                {
                                    Console.WriteLine("Access denied. You are not logged in as the assigned user.");
                                    Console.ReadKey();
                                }
                                break;
                            }                          
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Please fill in a valid ID");
                        Console.ReadKey();
                    }
                    break;
                case "5":
                    string changeTaskStatus = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Choose your Status")
                        .AddChoices("To-Do", "In Progress", "Done"));

                    Console.WriteLine("Enter ID to change: ");
                    
                    if (int.TryParse(Console.ReadLine(), out int changeidStr))
                    {
                        TaskItem task = myCollection.FindBy<int>(changeidStr, (x, id) => x.Id == id);
                        if (task == default)
                        {
                            Console.WriteLine("Task not found.");
                            Console.ReadKey();
                            break;
                        }

                        bool HasSameAssignee2 = false;
                        foreach(var assignee in task.Assignees)
                        {
                            if(currentUser == assignee)
                            {
                                HasSameAssignee2 = true ;
                            }
                        }
                        if (task.Assignees.Length != 0 && !HasSameAssignee2)
                        {
                            Console.WriteLine("Access denied. You are not logged in as the assigned user.");
                            Console.ReadKey();
                            break;
                        }

                        TaskItem taskToStatusChange = myCollection.FindBy<int>(changeidStr, (taskItem, id) => taskItem.Id == id);
                        var tasksToCompare = myCollection;
                        MyArray<int> prevTasks = [];
                        bool hasIncompletePreviousTask = false;
                        
                        foreach (var t in task.Previous)
                        {
                            foreach (var t2 in tasksToCompare)
                            {
                                if (t == t2.Id)
                                {
                                    prevTasks.Add(t);
                                }
                            }
                        }

                        if (changeTaskStatus == "Done")
                        {
                            
                            foreach (var t in prevTasks)
                            {
                                foreach (var t2 in tasksToCompare)
                                {
                                    if (t == t2.Id)
                                    {
                                        if (t2 != null && !t2.Completed)
                                        {
                                            hasIncompletePreviousTask = true;
                                        }
                                    }
                                }
                            }
                            if (hasIncompletePreviousTask)
                            {
                                Console.WriteLine("You cannot mark this task as Done until the previous task is completed.");
                                Console.ReadKey();
                                break;
                            }

                            _service.ToggleTaskCompletion(changeidStr);
                        }
                        taskToStatusChange.Status = changeTaskStatus;
                        _service.ChangeTaskStatus(changeidStr, changeTaskStatus);
                        
                    }
                    else
                    {
                        Console.WriteLine("Please fill in a valid ID");
                        Console.ReadKey();
                    }
                    break;
                case "6":
                    string ToggleFilter = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Choose filter type")
                        .AddChoices("Priority", "Status", "Creation Date", "Off"));

                    if(ToggleFilter == "Priority")
                    {
                        filterType = "Priority";
                        string ToggleFilter2 = AnsiConsole.Prompt(new SelectionPrompt<string>()
                            .Title("Choose priority")
                            .AddChoices("Low", "Middle", "High"));
                        filterString = ToggleFilter2;
                        filter = true;
                    }
                    else if(ToggleFilter == "Status")
                    {
                        filterType = "Status";
                        string ToggleFilter2 = AnsiConsole.Prompt(new SelectionPrompt<string>()
                            .Title("Choose status")
                            .AddChoices("To-Do", "In Progress", "Done"));
                        filterString = ToggleFilter2;
                        filter = true;
                    }
                    else if(ToggleFilter == "Off")
                    {
                        filter = false;
                    }
                    break;
                case "7":
                    string previousChoice = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Add or Remove Previous Task")
                        .AddChoices("Add", "Remove All"));

                    string currentTaskIdStr = Prompt("Enter task id to assign/remove task dependency: ");
                    if (!int.TryParse(currentTaskIdStr, out int currentTaskId))
                    {
                        Console.WriteLine("Please fill in a valid task ID");
                        Console.ReadKey();
                        break;
                    }

                    var allTasks = myCollection;
                    TaskItem? selectedTask = myCollection.FindBy<int>(currentTaskId, (taskItem, id) => taskItem.Id == id);

                    // foreach (var item in allTasks)
                    // {
                    //     if (item.Id == currentTaskId)
                    //     {
                    //         selectedTask = myCollection.FindBy<int>(currentTaskId, (taskItem, id) => taskItem.Id == id);
                    //         break;
                    //     }
                    // }

                    if (selectedTask == null)
                    {
                        Console.WriteLine("Task not found.");
                        Console.ReadKey();
                        break;
                    }

                    if (previousChoice == "Add")
                    {
                        string previousTaskIdStr = Prompt("Enter previous task id: ");
                        if (!int.TryParse(previousTaskIdStr, out int previousTaskId))
                        {
                            Console.WriteLine("Please fill in a valid previous task ID");
                            Console.ReadKey();
                            break;
                        }

                        if (previousTaskId == selectedTask.Id)
                        {
                            Console.WriteLine("A task cannot be its own previous task.");
                            Console.ReadKey();
                            break;
                        }

                        bool previousTaskExists = false;
                        foreach (var item in allTasks)
                        {
                            if (item.Id == previousTaskId)
                            {
                                previousTaskExists = true;
                                break;
                            }
                        }

                        if (!previousTaskExists)
                        {
                            Console.WriteLine("Previous task not found.");
                            Console.ReadKey();
                            break;
                        }

                        bool alreadyAssigned = false;
                        foreach (var prev in selectedTask.Previous)
                        {
                            if (prev == previousTaskId)
                            {
                                alreadyAssigned = true;
                                break;
                            }
                        }

                        if (alreadyAssigned)
                        {
                            Console.WriteLine("Dependency already assigned.");
                            Console.ReadKey();
                            break;
                        }

                        int[] newPrevious = new int[selectedTask.Previous.Length + 1];

                        for (int i = 0; i < selectedTask.Previous.Length; i++)
                        {
                            newPrevious[i] = selectedTask.Previous[i];
                        }

                        newPrevious[newPrevious.Length - 1] = previousTaskId;
                        selectedTask.Previous = newPrevious;
                        _service.ChangeTaskPrevious(currentTaskId, previousTaskId);

                        Console.WriteLine("Dependency task assigned.");
                        Console.ReadKey();
                    }
                    else
                    {
                        selectedTask.Previous = Array.Empty<int>();;
                        _service.ChangeTaskPrevious(currentTaskId, null);   
                        Console.WriteLine("Dependency task removed.");
                        Console.ReadKey();
                    }
                    break;
                case "8":
                    Console.WriteLine();
                    System.Console.WriteLine("=== Dependencies ===");
                    System.Console.WriteLine();
                    foreach(var task in myCollection) 
                    {
                        if (task.Previous != null && task.Previous.Length > 0)
                        {
                            var prevString = String.Join(", ", task.Previous);
                            Console.WriteLine($"To do Task: {task.Id} ---> Must do Task {prevString} first");
                        }
                    }
                    System.Console.WriteLine();
                    System.Console.WriteLine("=== Assignees ===");
                    System.Console.WriteLine();
                    foreach (var task in myCollection)
                    { 
                        if (task.Assignees != null)
                        {
                            var assignString = String.Join(", ", task.Assignees);
                            System.Console.WriteLine($"Assigned member for task {task.Id}: {assignString}");
                        }
                    }
                    Console.ReadLine();
                    break;
                case "9":
                    // JsonTaskRepository JsonTaskRepository = new JsonTaskRepository("tasks.json");
                    // JsonTaskRepository.SaveTasks(myCollection);
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}