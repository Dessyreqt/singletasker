namespace SingleTasker.Common;

using System.Diagnostics;

public class SingleTaskRepository
{
    private readonly string _path;
    private string? _directory;

    public SingleTaskRepository(string path)
    {
        _path = path;
    }

    public FileSystemWatcher? Watcher { get; private set; }

    public async Task Initialize()
    {
        await CreatePathIfNeeded();

        var filename = Path.GetFileName(_path);

        Watcher = new FileSystemWatcher(_directory);
        Watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess |
                               NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
        Watcher.Filter = filename;
        Watcher.IncludeSubdirectories = false;
        Watcher.EnableRaisingEvents = true;
    }

    public async Task<SingleTaskList?> GetTaskList(SingleTaskList? currentTaskList)
    {
        await CreatePathIfNeeded();
        var fileStream = await WaitForFile(_path, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new StreamReader(fileStream);
        var readTaskList = await reader.ReadToEndAsync();

        if (readTaskList != currentTaskList?.ToString())
        {
            return new SingleTaskListParser().Parse(readTaskList);
        }

        return null;
    }

    public void OpenFile()
    {
        var startInfo = new ProcessStartInfo { FileName = _path, UseShellExecute = true };
        Process.Start(startInfo);
    }

    public async Task SaveTaskList(SingleTaskList taskList)
    {
        await using var writer = new StreamWriter(_path, false);
        await writer.WriteAsync(taskList.ToString());
    }

    private async Task<FileStream> WaitForFile(string fullPath, FileMode mode, FileAccess access, FileShare share)
    {
        for (int numTries = 0; numTries < 10; numTries++)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(fullPath, mode, access, share);
                return fs;
            }
            catch (IOException)
            {
                if (fs is not null)
                {
                    await fs.DisposeAsync();
                }

                await Task.Delay(50);
            }
        }

        return null;
    }

    private async Task CreatePathIfNeeded()
    {
        _directory = Path.GetDirectoryName(_path);

        if (!Directory.Exists(_directory))
        {
            Directory.CreateDirectory(_directory);
        }

        if (File.Exists(_path))
        {
            return;
        }

        await using var writer = new StreamWriter(_path, false);
        await writer.WriteAsync(
            @"# SingleTasker todo

- Check the box to complete a task
- Use Up and Down arrows to move through task list
- Click the pencil to edit the task list using your favorite editor");
    }
}
