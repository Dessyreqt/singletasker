namespace SingleTasker.Common;

public class SingleTask
{
    public SingleTask(string description)
    {
        Description = description;
    }

    public SingleTask(string description, bool complete)
    {
        Description = description;
        Complete = complete;
    }

    public string Description { get; set; }
    public bool Complete { get; set; }

    public override string ToString()
    {
        return $"- {(Complete ? "[x] " : string.Empty)}{Description}";
    }
}
