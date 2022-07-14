namespace SingleTasker.Wpf.Common;

public class SingleTaskListParser
{
    private const string completedTaskStart = "- [x]";

    public SingleTaskList Parse(string rawList)
    {
        SingleTaskList taskList = new();
        SingleTaskListSection currentTaskListSection = null;

        foreach (var line in rawList.Split(Environment.NewLine))
        {
            var trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("#"))
            {
                if (currentTaskListSection is not null)
                {
                    taskList.Sections.Add(currentTaskListSection);
                }

                currentTaskListSection = new SingleTaskListSection { Title = line.TrimStart('#').Trim() };
                continue;
            }

            if (currentTaskListSection is null)
            {
                continue;
            }

            if (trimmedLine.StartsWith(completedTaskStart))
            {
                var description = trimmedLine.Remove(0, completedTaskStart.Length).Trim();

                currentTaskListSection.Items.Add(new SingleTask(description, true));
            }
            else if (trimmedLine.StartsWith("-"))
            {
                var description = trimmedLine.TrimStart('-').Trim();

                currentTaskListSection.Items.Add(new SingleTask(description));
            }
        }

        if (currentTaskListSection is not null)
        {
            taskList.Sections.Add(currentTaskListSection);
        }

        return taskList;
    }
}
