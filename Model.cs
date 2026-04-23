public class TaskItem : IComparable<TaskItem>
{
    public int Id { get; set; }
    public required string Description { get; set; }
    public bool Completed { get; set; }
    public string Priority { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Status { get; set; } = "To-Do";
    public string[] Assignees { get; set; } = Array.Empty<string>();
    public int[] Previous { get; set; } = Array.Empty<int>();

    public int CompareTo(TaskItem other)
    {
        if (other == null) return 1;
        return Id.CompareTo(other.Id);
    }
}