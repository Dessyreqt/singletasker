namespace SingleTasker.Wpf.Common;

using System.Text;

public class SingleTaskList
{
    public SingleTaskList()
    {
        Sections = new List<SingleTaskListSection>();
    }

    public List<SingleTaskListSection> Sections { get; }

    public override string ToString()
    {
        StringBuilder builder = new();

        foreach (var section in Sections)
        {
            if (builder.Length > 0)
            {
                builder.AppendLine();
            }

            builder.AppendLine(section.ToString());
        }

        return builder.ToString();
    }
}
