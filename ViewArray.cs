using System;
using System.Collections.Generic;
using System.Data.Common;
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
        return Console.ReadLine();
    }

    public void Run() 
    {
        IMyCollection<TaskItem> myCollection = new MyArray<TaskItem>();
        string currentDataType = "Array";
        bool filter = false;
        string filterString = "";
        string filterType = "";
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
            Console.WriteLine("\nOptions:");
            Console.WriteLine("0. Change Datatype");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Remove Task");
            Console.WriteLine("3. Add or Remove Assignees");
            Console.WriteLine("4. Change Task Priority");
            Console.WriteLine("5. Change Task Status");
            Console.WriteLine("6. Toggle Filter");
            Console.WriteLine("8. Exit");

            string option = Prompt("Select an option: ");
            switch (option) {
                case "0":
                    var selectedDataType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Select DataType")
                        .AddChoices("Array", "Linked List"));

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
                    Console.ReadKey();
                    break;
                case "1":
                    string description = Prompt("Enter task description: ");
                    string priority = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Choose your priority")
                        .AddChoices("Low", "Middle", "High"));

                    var arr = _service.GetAllTasks().ToArray();
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
                        _service.RemoveTask(removeId);
                    }

                    TaskItem ItemToRemove = myCollection.FindBy<int>(removeId, (item, Id) => item.Id == Id);
                    
                    if(ItemToRemove != default)
                    {
                        myCollection.Remove(ItemToRemove);
                        System.Console.WriteLine($"ID: {removeId} has been deleted");
                    }
                    else
                        System.Console.WriteLine("No task with given ID");;
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
                            System.Console.WriteLine("Enter name:");
                            string name = Console.ReadLine()!;
                            ItemToAddAssignees.Assignees.Add(name);

                            _service.ChangeTaskAssignees(taskID, name, true);
                        }
                        else
                        {
                            TaskItem ItemToAddAssignees = myCollection.FindBy<int>(taskID, (item, Id) => item.Id == Id);
                            if(ItemToAddAssignees == null)
                            {
                                System.Console.WriteLine("Doesn't exist!");
                                Console.ReadKey();
                                break;
                            }
                            System.Console.WriteLine("Enter name:");
                            string name = Console.ReadLine()!;
                            ItemToAddAssignees.Assignees.Remove(name);

                            _service.ChangeTaskAssignees(taskID, name, false);
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
                        var array = _service.GetAllTasks().ToArray();
                        foreach(var item in array)
                        {
                            if(item.Id == changeIdStr)
                            {
                                _service.ChangeTaskPriority(changeIdStr, priorityChange);
                                TaskItem TaskToChange = myCollection.FindBy<int>(changeIdStr, (TaskItem, id) => TaskItem.Id == id);
                                TaskToChange.Priority = priorityChange;
                            }
                        }
                        System.Console.WriteLine("Please fill in a valid ID");
                        Console.ReadKey();
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

                    System.Console.WriteLine("Enter ID to change: ");
                    if (int.TryParse(Console.ReadLine(), out int changeidStr))
                    {
                        var array = _service.GetAllTasks().ToArray();
                        foreach(var item in array)
                        {
                            if(item.Id == changeidStr && changeTaskStatus == "Done")
                            {
                                TaskItem TaskToStatusChange = myCollection.FindBy<int>(changeidStr, (TaskItem, id) => TaskItem.Id == id);
                                _service.ToggleTaskCompletion(changeidStr);
                                _service.ChangeTaskStatus(changeidStr, changeTaskStatus);
                                TaskToStatusChange.Status = changeTaskStatus;
                            }
                            if(item.Id == changeidStr && changeTaskStatus == "In Progress")
                            {
                                TaskItem TaskToStatusChange = myCollection.FindBy<int>(changeidStr, (TaskItem, id) => TaskItem.Id == id);
                                _service.ChangeTaskStatus(changeidStr, changeTaskStatus);
                                TaskToStatusChange.Status = changeTaskStatus;
                            }
                            if(item.Id == changeidStr && changeTaskStatus == "To-Do")
                            {
                                TaskItem TaskToStatusChange = myCollection.FindBy<int>(changeidStr, (TaskItem, id) => TaskItem.Id == id);
                                _service.ChangeTaskStatus(changeidStr, changeTaskStatus);
                                TaskToStatusChange.Status = changeTaskStatus;
                            }
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("Please fill in a valid ID");
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
                    else if(ToggleFilter == "Creation Date")
                    {
                        filterType = "Sort";
                        filter = true;
                    }
                    else if(ToggleFilter == "Off")
                    {
                        filter = false;
                    }
                    break;
                case "7":
                    break;
                case "8":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}