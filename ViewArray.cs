using System;
using System.Collections.Generic;
using System.Data.Common;

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
        while (true) 
        {
            Console.Clear();
            System.Console.WriteLine("-------------------------------------------");
            if (myArray.Count() == 0)
            {
                System.Console.WriteLine("NO TASK");
            }
            else
                DisplayTasks(_service.GetAllTasks());
            System.Console.WriteLine("-------------------------------------------");

            Console.WriteLine("\n==== ToDo List ====");
            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Remove Task");
            Console.WriteLine("3. Toggle Task State");
            Console.WriteLine("4. Exit");

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

                    var arr = _service.GetAllTasks().ToArray();
                    int newId = 1;

                    for (int i = 0; i < arr.Length; i++)
                    {
                        if (arr[i] != null && arr[i].Id >= newId)
                            newId = arr[i].Id + 1;
                    }

                    TaskItem newTask = new TaskItem { Id = newId, Description = description, Completed = false };
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
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}