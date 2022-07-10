namespace SingleTasker.Common;

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
            writer.Write(@"# SingleTasker todo

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
