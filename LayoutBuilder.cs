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
                todoTable.AddRow($"[bold]{items[i].Id}[/] {items[i].Description}");
            }
            else if (items[i].Status == "In Progress")
            {
                inProgressTable.AddRow($"[bold]{items[i].Id}[/] {items[i].Description}");
            }
            else if (items[i].Status == "Done")
            {
                doneTable.AddRow($"[bold]{items[i].Id}[/] {items[i].Description}");
            }
        }

        var columns = new Columns(todoTable, inProgressTable, doneTable);
        var borderedLayout = new Panel(columns)
            .BorderColor(Color.Blue)
            .Header(new PanelHeader("Kanban Board", Justify.Center))
            .Expand()
            .Padding(2, 2);

        AnsiConsole.Write(borderedLayout);
        Console.ReadKey();
    }
}
