namespace SingleTasker.Common;

public class SingleTaskList
{
    public SingleTaskList()
    {
        Title = string.Empty;
        Items = new List<SingleTask>();
    }

    public string Title { get; set; }
    public int CurrentItem { get; set; }
    public List<SingleTask> Items { get; }
}
