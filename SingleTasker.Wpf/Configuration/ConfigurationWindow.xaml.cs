namespace SingleTasker.Wpf.Configuration;

using System.Windows;

/// <summary>
/// Interaction logic for ConfigurationWindow.xaml
/// </summary>
public partial class ConfigurationWindow : Window
{
    private readonly ConfigurationRepository _config;

    public ConfigurationWindow(ConfigurationRepository config)
    {
        _config = config;
        InitializeComponent();
    }

    private void OpenConfigButton_Click(object sender, RoutedEventArgs e)
    {
        _config.OpenFile();
    }
}
