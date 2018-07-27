using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveServices.Models_img
{

    public class ImageSearchDto
    {
        public string _type { get; set; }
        public Instrumentation Instrumentation { get; set; }
        public string ReadLink { get; set; }
        public string WebSearchUrl { get; set; }
        public int TotalEstimatedMatches { get; set; }
        public int NextOffset { get; set; }
        public Value[] Value { get; set; }
    }

    public class Instrumentation
    {
        public string _type { get; set; }
    }

    public class Value
    {
        public string WebSearchUrl { get; set; }
        public string Name { get; set; }
        public string ThumbnailUrl { get; set; }
        public DateTime DatePublished { get; set; }
        public string ContentUrl { get; set; }
        public string HostPageUrl { get; set; }
        public string ContentSize { get; set; }
        public string EncodingFormat { get; set; }
        public string HostPageDisplayUrl { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Thumbnail Thumbnail { get; set; }
        public string ImageInsightsToken { get; set; }
        public Insightsmetadata InsightsMetadata { get; set; }
        public string ImageId { get; set; }
        public string AccentColor { get; set; }
    }

    public class Thumbnail
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Insightsmetadata
    {
        public int PagesIncludingCount { get; set; }
        public int AvailableSizesCount { get; set; }
    }

}
