namespace SingleTasker.Common;

public class SingleTaskListParser
{
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

            if (trimmedLine.StartsWith("-"))
            {
                var description = trimmedLine.TrimStart('-').Trim();

                taskList.Items.Add(new SingleTask(description));
            }
        }

        return taskList;
    }
}
