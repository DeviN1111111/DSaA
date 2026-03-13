using System.Security.Cryptography.X509Certificates;

public class TaskItem : IComparable<TaskItem>
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public bool Completed { get; set; }

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