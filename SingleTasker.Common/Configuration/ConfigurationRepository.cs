namespace SingleTasker.Common.Configuration;

using System.ComponentModel;
using System.Diagnostics;
using Newtonsoft.Json;

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
        var startInfo = new ProcessStartInfo
        {
            FileName = _path,
            UseShellExecute = true
        };
        try { Process.Start(startInfo); }
        catch (Win32Exception ex) { }
    }

    public async Task<AppConfiguration> GetConfiguration()
    {
        await CreatePathIfNeeded();
        using var reader = new StreamReader(_path);
        var readConfiguration = await reader.ReadToEndAsync();

        return JsonConvert.DeserializeObject<AppConfiguration>(readConfiguration, SerializationSettings.Configuration) ?? new AppConfiguration();
    }

    public async Task SaveConfiguration(AppConfiguration config)
    {
        await CreatePathIfNeeded();
        await using var writer = new StreamWriter(_path);
        var writeConfiguration = JsonConvert.SerializeObject(config, SerializationSettings.Configuration);
        await writer.WriteAsync(writeConfiguration);
    }

    private async Task CreatePathIfNeeded()
    {
        var directory = Path.GetDirectoryName(_path);

        if (!Directory.Exists(directory)) { Directory.CreateDirectory(directory); }

        if (File.Exists(_path)) { return; }

        await using var writer = new StreamWriter(_path);
        var writeConfiguration = JsonConvert.SerializeObject(new AppConfiguration(), SerializationSettings.Configuration);
        await writer.WriteAsync(writeConfiguration);
    }
}
