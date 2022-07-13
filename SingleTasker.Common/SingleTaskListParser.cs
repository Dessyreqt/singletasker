namespace SingleTasker.Common;

public class SingleTaskListParser
{
    private const string completedTaskStart = "- [x]";

    public SingleTaskList Parse(string rawList)
    {
        SingleTaskList taskList = new();

        foreach (var line in rawList.Split(Environment.NewLine))
        {
            var trimmedLine = line.Trim();

            if (trimmedLine.StartsWith("#"))
            {
                if (taskList.Title == string.Empty)
                {
                    taskList.Title = line.TrimStart('#').Trim();
                }
                else
                {
                    return taskList;
                }
            }

            if (trimmedLine.StartsWith(completedTaskStart))
            {
                var description = trimmedLine.Remove(0, completedTaskStart.Length).Trim();

                taskList.Items.Add(new SingleTask(description, true));
            }
            else if (trimmedLine.StartsWith("-"))
            {
                var description = trimmedLine.TrimStart('-').Trim();

                taskList.Items.Add(new SingleTask(description));
            }
        }

        return taskList;
    }
}
