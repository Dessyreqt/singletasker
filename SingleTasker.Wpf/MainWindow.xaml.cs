namespace SingleTasker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SingleTasker.Common;
using Path = System.IO.Path;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly SingleTaskRepository _repo;
    private int _currentTaskIndex;
    private SingleTaskList _taskList;
    private SingleTask _currentTask;

    public MainWindow()
    {
        InitializeComponent();
        var storagePath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "SingleTasker", "tasks.md");
        _repo = new SingleTaskRepository(storagePath);
    }

    private async void TaskListChanged(object sender, FileSystemEventArgs e)
    {
        await Dispatcher.InvokeAsync(LoadTaskList);
    }

    private async Task LoadTaskList()
    {
        _taskList = await _repo.GetTaskList();
        NextIncompleteTask();
    }

    private void DisplayCurrentTask()
    {
        TaskDescriptionLabel.Content = _currentTask.Description;
        TaskCompleteCheckbox.IsChecked = _currentTask.Complete;
    }

    private void SetCurrentTask()
    {
        if (_currentTaskIndex == -1)
        {
            _currentTaskIndex = _taskList.GetFirstIncompleteTask();
        }

        _currentTask = _taskList.Items[_currentTaskIndex];
    }

    private void UpButton_Click(object sender, RoutedEventArgs e)
    {
        PreviousTask();
    }

    private void DownButton_Click(object sender, RoutedEventArgs e)
    {
        NextTask();
    }

    private void PreviousTask()
    {
        if (_currentTaskIndex > 0)
        {
            _currentTaskIndex--;
        }

        SetCurrentTask();
        DisplayCurrentTask();
    }

    private void NextTask()
    {
        if (_currentTaskIndex < _taskList.Items.Count - 1)
        {
            _currentTaskIndex++;
        }

        SetCurrentTask();
        DisplayCurrentTask();
    }

    private void NextIncompleteTask()
    {
        _currentTaskIndex = -1;
        SetCurrentTask();
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
            NextTask();
        }
    }

    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {
        await _repo.Initialize();
        _repo.Watcher.Changed += TaskListChanged;
        await LoadTaskList();
    }
}
