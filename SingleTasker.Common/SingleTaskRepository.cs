namespace SingleTasker.Common;

using System.Diagnostics;

public class SingleTaskRepository
{
    private readonly string _path;

    public SingleTaskRepository(string path)
    {
        _path = path;
    }

    public void CreatePathIfNeeded()
    {
        var directory = Path.GetDirectoryName(_path);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(_path))
        {
            using var writer = new StreamWriter(_path, false);
            writer.Write(
                @"# SingleTasker todo

- Load from a file
- Display task on screen
- Checkbox on task
- back/forward buttons
- sections
- save task list wherever
- reload task list on edit
- save task list on checked");
        }
    }

    public SingleTaskList GetTaskList()
    {
        CreatePathIfNeeded();
        using var reader = new StreamReader(_path);
        return new SingleTaskListParser().Parse(reader.ReadToEnd());
    }

    public void OpenFile()
    {
        var startInfo = new ProcessStartInfo { FileName = _path, UseShellExecute = true };
        Process.Start(startInfo);
    }

    public void SaveTaskList(SingleTaskList taskList)
    {
        using var writer = new StreamWriter(_path, false);
        writer.Write(taskList.ToString());
    }
}

/*
# SingleTasker todo

- Load from a file
- Display task on screen
- Checkbox on task
- back/forward buttons
- sections
- save task list wherever
- reload task list on edit
- save task list on checked

 */
