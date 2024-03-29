﻿namespace SingleTasker.Wpf;

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using SingleTasker.Common;
using SingleTasker.Common.Configuration;
using SingleTasker.Wpf.Configuration;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private ConfigurationRepository _configRepo;
    private SingleTaskRepository _repo;
    private int _currentTaskIndex;
    private int _currentSectionIndex;
    private SingleTaskList _taskList;
    private SingleTaskListSection _currentTaskListSection;
    private SingleTask _currentTask;
    private bool _loaded;

    public MainWindow()
    {
        InitializeComponent();
    }

    private async void TaskListChanged(object sender, FileSystemEventArgs e)
    {
        await Dispatcher.InvokeAsync(() => LoadTaskList(_currentSectionIndex));
    }

    private async Task LoadTaskList(int selectedSectionIndex = -1)
    {
        var readTaskList = await _repo.GetTaskList(_taskList);

        if (readTaskList == null)
        {
            return;
        }

        _taskList = readTaskList ?? _taskList;
        SectionsComboBox.Items.Clear();
        _taskList.Sections.ForEach(x => SectionsComboBox.Items.Add(x.Title));

        if (selectedSectionIndex == -1)
        {
            var firstSection = _taskList.Sections.First();
            SectionsComboBox.SelectedItem = firstSection.Title;
            _currentTaskListSection = firstSection;
        }
        else
        {
            SectionsComboBox.SelectedIndex = selectedSectionIndex;
            _currentTaskListSection = _taskList.Sections[selectedSectionIndex];
        }

        await NextIncompleteTask();
    }

    private void DisplayCurrentTask()
    {
        TaskDescriptionLabel.Text = _currentTask.Description;
        TaskCompleteCheckbox.IsChecked = _currentTask.Complete;
    }

    private async Task SetCurrentTask()
    {
        if (_currentTaskListSection.Items.Count == 0)
        {
            _currentTaskListSection.Items.Add(new SingleTask("Add more things to do"));
            _currentTaskIndex = 0;
            await _repo.SaveTaskList(_taskList);
        }

        if (_currentTaskIndex == -1)
        {
            _currentTaskIndex = _currentTaskListSection.GetFirstIncompleteTask();
        }

        _currentTask = _currentTaskListSection.Items[_currentTaskIndex];
    }

    private async void UpButton_Click(object sender, RoutedEventArgs e)
    {
        await PreviousTask();
    }

    private async void DownButton_Click(object sender, RoutedEventArgs e)
    {
        await NextTask();
    }

    private async Task PreviousTask()
    {
        if (_currentTaskIndex > 0)
        {
            _currentTaskIndex--;
        }

        await SetCurrentTask();
        DisplayCurrentTask();
    }

    private async Task NextTask()
    {
        if (_currentTaskIndex < _currentTaskListSection.Items.Count - 1)
        {
            _currentTaskIndex++;
        }

        await SetCurrentTask();
        DisplayCurrentTask();
    }

    private async Task NextIncompleteTask()
    {
        _currentTaskIndex = -1;
        await SetCurrentTask();
        DisplayCurrentTask();
    }

    private void EditButton_Click(object sender, RoutedEventArgs e)
    {
        _repo.OpenFile();
    }

    private async void TaskCompleteCheckbox_Click(object sender, RoutedEventArgs e)
    {
        _currentTask.Complete = TaskCompleteCheckbox.IsChecked ?? false;
        await _repo.SaveTaskList(_taskList);

        if (_currentTask.Complete)
        {
            await NextTask();
        }
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        var configPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "SingleTasker", "config.json");
        _configRepo = new ConfigurationRepository(configPath);
        await _configRepo.Initialize();

        var config = await _configRepo.GetConfiguration();
        _repo = new SingleTaskRepository(config.TaskListPath);
        await _repo.Initialize();

        Top = config.Top ?? Top;
        Left = config.Left ?? Left;
        Height = config.Height ?? Height;
        Width = config.Width ?? Width;

        _repo.Watcher.Changed += TaskListChanged;
        await LoadTaskList();

        if (config.SelectedTaskList is not null && SectionsComboBox.Items.Contains(config.SelectedTaskList))
        {
            SectionsComboBox.SelectedValue = config.SelectedTaskList;
            _currentSectionIndex = SectionsComboBox.SelectedIndex;
            _currentTaskListSection = _taskList.Sections[_currentSectionIndex];
            await NextIncompleteTask();
        }

        _loaded = true;
    }

    private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Environment.Exit(0);
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private async void SectionsComboBox_DropDownClosed(object sender, EventArgs e)
    {
        if (SectionsComboBox.SelectedIndex == -1)
        {
            return;
        }

        _currentSectionIndex = SectionsComboBox.SelectedIndex;
        _currentTaskListSection = _taskList.Sections[_currentSectionIndex];
        var nextIncompleteTask = NextIncompleteTask();

        if (_loaded)
        {
            var saveConfigurationTask = SaveConfiguration();
            await saveConfigurationTask;
        }

        await nextIncompleteTask;
    }

    private void ConfigButton_Click(object sender, RoutedEventArgs e)
    {
        var configWindow = new ConfigurationWindow(_configRepo);
        configWindow.ShowDialog();
    }

    private async void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (!_loaded)
        {
            return;
        }

        var newWidth = Width;
        var newHeight = Height;

        await Task.Delay(1000);

        if (newWidth != Width || newHeight != Height)
        {
            return;
        }

        await SaveConfiguration();
    }

    private async Task SaveConfiguration()
    {
        if (_configRepo is null)
        {
            return;
        }

        var config = await _configRepo.GetConfiguration();

        config.Top = Top;
        config.Left = Left;
        config.Width = Width;
        config.Height = Height;
        config.SelectedTaskList = SectionsComboBox.Text;

        await _configRepo.SaveConfiguration(config);
    }

    private async void Window_LocationChanged(object sender, EventArgs e)
    {
        if (!_loaded)
        {
            return;
        }

        var newTop = Top;
        var newLeft = Left;

        await Task.Delay(1000);

        if (newTop != Top || newLeft != Left)
        {
            return;
        }

        await SaveConfiguration();
    }
}
