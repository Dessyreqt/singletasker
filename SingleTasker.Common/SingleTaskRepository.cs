namespace SingleTasker.Common;

public class SingleTaskRepository
{
    public SingleTaskList GetTaskList(string path)
    {
        using (var reader = new StreamReader(path))
        {
            return (new SingleTaskListParser()).Parse(reader.ReadToEnd());
        }
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
