using System.Collections.Generic;

namespace Base.PlotSystem
{
    public class Topic
    {
        public string TopicName;
        public List<Plot> Plots;

        public Topic()
        {
            Plots = new List<Plot>();
        }
    }
}