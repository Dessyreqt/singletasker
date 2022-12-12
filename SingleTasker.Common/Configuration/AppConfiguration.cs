namespace SingleTasker.Common.Configuration;

public class AppConfiguration
{
    public string TaskListPath { get; set; } = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "SingleTasker", "tasks.md");
    public double? Height { get; set; } = null;
    public double? Width { get; set; } = null;
    public double? Top { get; set; } = null;
    public double? Left { get; set; } = null;
    public string? SelectedTaskList { get; set; } = null;
}
