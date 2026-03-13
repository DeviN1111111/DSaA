using System;
using System.Collections.Generic;

interface ITaskView
{
    void Run();
}

public class ConsoleTaskView : ITaskView 
{
    private readonly ITaskService _service;

    public ConsoleTaskView(ITaskService service) {
        _service = service;
    }

    void DisplayTasks(IEnumerable<TaskItem> tasks) {
        Console.Clear();
        Console.WriteLine("==== ToDo List ====");
        foreach (var task in tasks)
            Console.WriteLine($"{task}");
    }

    string Prompt(string prompt) {
        Console.Write(prompt);
        return Console.ReadLine();
    }

    public void Run() 
    {
        MyArray<TaskItem> myArray = new MyArray<TaskItem>();
        while (true) {
            // DisplayTasks(_service.GetAllTasks());
            Console.WriteLine($"{myArray.Count()}");
            myArray.PrintAll();

            Console.WriteLine("\n==== ToDo List ====");
            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. Remove Task");
            Console.WriteLine("3. Toggle Task State");
            Console.WriteLine("4. Exit");

            string option = Prompt("Select an option: ");
            switch (option) {
                case "1":
                    string description = Prompt("Enter task description: ");

                    TaskItem newTask = new TaskItem { Id = 1, Description = description, Completed = false };
                    myArray.Add(newTask);

                    _service.AddTask(description);
                    break;

                case "2":
                    string removeIdStr = Prompt("Enter task id to remove: ");
                    if (int.TryParse(removeIdStr, out int removeId)) {
                        _service.RemoveTask(removeId);
                    }

                    
                    break;

                case "3":
                    string toggleIdStr = Prompt("Enter task id to toggle: ");
                    if (int.TryParse(toggleIdStr, out int toggleId)) {
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