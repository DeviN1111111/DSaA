using Spectre.Console;

public class LayoutBuilder<T> where T : TaskItem
{
    public static void RenderLayout(T[] items)
    {
        var todoTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Yellow)
            .Title("To-Do")
            .AddColumn("Tasks");

        var inProgressTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Orange1)
            .Title("In Progress")
            .AddColumn("Tasks");

        var doneTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Green)
            .Title("Done")
            .AddColumn("Tasks");

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null) continue;

            if (items[i].Status == "To-Do")
            {   
                if (items[i].Assignees != null && items[i].Assignees.Length > 0)
                {
                    string assigneesStr = string.Join(", ", items[i].Assignees);
                    todoTable.AddRow($"[bold]{items[i].Id}[/] | {items[i].Description} | {items[i].Priority} | Members: {assigneesStr} | Previous Task: {items[i].Previous}");
                }
                else
                    todoTable.AddRow($"[bold]{items[i].Id}[/] | {items[i].Description} | {items[i].Priority} | Previous Task: {items[i].Previous}");
            }
            else if (items[i].Status == "In Progress")
            {
                if (items[i].Assignees != null && items[i].Assignees.Length > 0)
                {
                    string assigneesStr = string.Join(", ", items[i].Assignees);
                    inProgressTable.AddRow($"[bold]{items[i].Id}[/] | {items[i].Description} | {items[i].Priority} | Members: {assigneesStr} | Previous Task: {items[i].Previous}");
                }
                else
                    inProgressTable.AddRow($"[bold]{items[i].Id}[/] | {items[i].Description} | {items[i].Priority} | Previous Task: {items[i].Previous}");
            }
            else if (items[i].Status == "Done")
            {
                if (items[i].Assignees != null && items[i].Assignees.Length > 0)
                {
                    string assigneesStr = string.Join(", ", items[i].Assignees);
                    doneTable.AddRow($"[bold]{items[i].Id}[/] | {items[i].Description} | {items[i].Priority} | Members: {assigneesStr} | Previous Task: {items[i].Previous}");
                }
                else
                    doneTable.AddRow($"[bold]{items[i].Id}[/] | {items[i].Description} | {items[i].Priority} | Previous Task: {items[i].Previous}");
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
