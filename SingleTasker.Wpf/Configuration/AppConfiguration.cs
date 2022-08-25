namespace SingleTasker.Wpf.Configuration;

using System;
using System.IO;

public class AppConfiguration
{
    public string TaskListPath { get; set; } = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "SingleTasker", "tasks.md");
}
