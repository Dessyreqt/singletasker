namespace SingleTasker.Common;

public class SingleTask
{
    public SingleTask(string description)
    {
        Description = description;
    }

    public string Description { get; set; }
    public bool Complete { get; set; }
}
