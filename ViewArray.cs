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
        MyArray<TaskItem> myArray = new MyArray<TaskItem>();
        bool filter = false;
        string filterString = "";
        string filterType = "";
        while (true) 
        {
            Console.Clear();
            MyArray<TaskItem> myFilterArray = myArray;
            if(filter)
            {
                if(filterType == "Sort")
                {
                    myFilterArray.Sort((a, b) => a.CreatedAt.CompareTo(b.CreatedAt));
                }
                else if(filterType == "Priority")
                {
                    myFilterArray = (MyArray<TaskItem>)myFilterArray.Filter((task) => task.Priority == filterString);
                }
                else if(filterType == "Status")
                {
                    myFilterArray = (MyArray<TaskItem>)myFilterArray.Filter((task) => task.Status == filterString);
                }
            }
            LayoutBuilder<TaskItem>.RenderLayout(myFilterArray.ToArray());

            Console.WriteLine("\n==== ToDo List ====");
            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Remove Task");
            Console.WriteLine("3. Toggle Task State");
            Console.WriteLine("4. Change Task Priority");
            Console.WriteLine("5. Change Task Status");
            Console.WriteLine("6. Toggle filter");
            Console.WriteLine("7. Exit");

            string option = Prompt("Select an option: ");
            switch (option) {
                case "1":
                    if(myArray.CountInArray() >= 5)
                    {
                        System.Console.WriteLine($"Array is full, max: {myArray.Count}");
                        Console.ReadKey();
                        break;
                    }
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
                    myArray.Add(newTask);
                    _service.AddTask(description);
                    break;
                case "2":
                    string removeIdStr = Prompt("Enter task id to remove: ");
                    if (int.TryParse(removeIdStr, out int removeId)) 
                    {
                        _service.RemoveTask(removeId);
                    }

                    TaskItem ItemToRemove = myArray.FindBy<int>(removeId, (item, Id) => item.Id == Id);
                    
                    if(ItemToRemove != default)
                    {
                        myArray.Remove(ItemToRemove);
                        System.Console.WriteLine($"ID: {removeId} has been deleted");
                    }
                    else
                        System.Console.WriteLine("No task with given ID");;
                        Console.ReadKey();
                    break;
                case "3":
                    string toggleIdStr = Prompt("Enter task id to toggle: ");
                    if (int.TryParse(toggleIdStr, out int toggleId)) 
                    {
                        _service.ToggleTaskCompletion(toggleId);
                    }
                    break;
                case "4":
                    string priorityChange = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Choose your priority")
                        .AddChoices("Low", "Middle", "High"));

                    System.Console.WriteLine("Enter ID to change: ");
                    int changeIdStr = Convert.ToInt32(Console.ReadLine());

                    TaskItem TaskToChange = myArray.FindBy<int>(changeIdStr, (TaskItem, id) => TaskItem.Id == id);
                    TaskToChange.Priority = priorityChange;
                    break;
                case "5":
                    string changeTaskStatus = AnsiConsole.Prompt(new SelectionPrompt<string>()
                        .Title("Choose your Status")
                        .AddChoices("To-Do", "In Progress", "Done"));

                    System.Console.WriteLine("Enter ID to change: ");
                    int changeidStr = Convert.ToInt32(Console.ReadLine());
                    TaskItem TaskToStatusChange = myArray.FindBy<int>(changeidStr, (TaskItem, id) => TaskItem.Id == id); 

                    TaskToStatusChange.Status = changeTaskStatus;
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
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}