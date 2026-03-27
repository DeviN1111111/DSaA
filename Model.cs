using System.Security.Cryptography.X509Certificates;

public class TaskItem : IComparable<TaskItem>
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public bool Completed { get; set; }
    public string Priority { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Status { get; set; } = "To-Do";
    public MyArray<string> Assignees = new MyArray<string>();
    public MyArray<TaskItem> myArray = new MyArray<TaskItem>();

    public int CompareTo(TaskItem other)
    {
        if (other == null) return 1;
        return Id.CompareTo(other.Id);
    }

    public override string ToString()
    {
        return $"[{Id}] {Description} - {(Completed ? "✓" : "✗")}";
    }
}