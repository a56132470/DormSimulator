using System.Collections.Generic;

public class Topic
{
    public string TopicName;
    public List<Plot> plots;

    public Topic()
    {
        plots = new List<Plot>();
    }
}