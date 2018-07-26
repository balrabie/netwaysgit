using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AzureServices
{
    public class ImageDto
    {
        public JToken Analysis { get; internal set; }

        public TimeSpan OccurenceTime { get; internal set; }

        public string DesiredTag { get; internal set; }
    }
}
