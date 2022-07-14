namespace SingleTasker.Wpf.Common;

using System.Text;

public class SingleTaskListSection
{
    public SingleTaskListSection()
    {
        Title = string.Empty;
        Items = new List<SingleTask>();
    }

    public List<SingleTask> Items { get; }

    public string Title { get; set; }

    public int GetFirstIncompleteTask()
    {
        var firstIncompleteTask = Items.FirstOrDefault(item => !item.Complete);

        if (firstIncompleteTask is null)
        {
            return Items.Count - 1;
        }

        return Items.IndexOf(firstIncompleteTask);
    }

    public override string ToString()
    {
        StringBuilder builder = new();

        builder.AppendLine($"# {Title}");
        builder.AppendLine();

        foreach (var item in Items)
        {
            builder.AppendLine(item.ToString());
        }

        return builder.ToString();
    }
}
