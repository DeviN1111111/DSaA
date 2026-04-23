using Spectre.Console;

public class LayoutBuilder<T> where T : TaskItem
{
    public static void RenderLayout(T[] items)
    {
        var todoTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Yellow)
            .Title("To-Do\n")
            .AddColumn("Tasks");

        var inProgressTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Orange1)
            .Title("In Progress")
            .AddColumn("Tasks");

        var doneTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Green)
            .Title("Done\n")
            .AddColumn("Tasks");

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null) continue;

            string assigneesStr = string.Join(", ", items[i].Assignees);
            string previousStr = string.Join(", ", items[i].Previous);

            if (items[i].Status == "To-Do")
            {   
                if (items[i].Assignees != null && items[i].Assignees.Length > 0)
                {
                    todoTable.AddRow($"[bold]{items[i].Id}[/] | {items[i].Description} | {items[i].Priority} | Assignees: {assigneesStr} | Previous Tasks: {previousStr}");
                }
                else
                    todoTable.AddRow($"[bold]{items[i].Id}[/] | {items[i].Description} | {items[i].Priority} | Previous Tasks: {previousStr}");
            }
            else if (items[i].Status == "In Progress")
            {
                if (items[i].Assignees != null && items[i].Assignees.Length > 0)
                {
                    inProgressTable.AddRow($"[bold]{items[i].Id}[/] | {items[i].Description} | {items[i].Priority} | Assignees: {assigneesStr} | Previous Tasks: {previousStr}");
                }
                else
                    inProgressTable.AddRow($"[bold]{items[i].Id}[/] | {items[i].Description} | {items[i].Priority} | Previous Tasks: {previousStr}");
            }
            else if (items[i].Status == "Done")
            {
                if (items[i].Assignees != null && items[i].Assignees.Length > 0)
                {
                    doneTable.AddRow($"[bold]{items[i].Id}[/] | {items[i].Description} | {items[i].Priority} | Assignees: {assigneesStr} | Previous Tasks: {previousStr}");
                }
                else
                    doneTable.AddRow($"[bold]{items[i].Id}[/] | {items[i].Description} | {items[i].Priority} | Previous Tasks: {previousStr}");
            }
        }

        var columns = new Columns(todoTable, inProgressTable, doneTable);
        var borderedLayout = new Panel(columns)
            .BorderColor(Color.Blue)
            .Header(new PanelHeader("Kanban Board", Justify.Center))
            .Expand()
            .Padding(2, 2);

        AnsiConsole.Write(borderedLayout);
        // Console.ReadKey();
    }
}
