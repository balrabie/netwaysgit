using Newtonsoft.Json.Linq;
using System;

namespace templibrary
{
    public class FrameDto
    {
        public JToken Analysis { get; internal set; }

        public TimeSpan OccurenceTime { get; internal set; }

        public string DesiredTag { get; internal set; }
    }
}