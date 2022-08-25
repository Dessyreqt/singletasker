namespace SingleTasker.Wpf.Configuration;

using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public class ConfigurationRepository
{
    private readonly string _path;

    public ConfigurationRepository(string path)
    {
        _path = path;
    }

    public async Task Initialize()
    {
        await CreatePathIfNeeded();
    }

    public void OpenFile()
    {
        var startInfo = new ProcessStartInfo { FileName = _path, UseShellExecute = true };
        Process.Start(startInfo);
    }

    private async Task CreatePathIfNeeded()
    {
        var directory = Path.GetDirectoryName(_path);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (File.Exists(_path))
        {
            return;
        }

        await using var writer = new StreamWriter(_path, false);
        await writer.WriteAsync(@"{}");
    }
}
