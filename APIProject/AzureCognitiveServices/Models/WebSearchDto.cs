using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveServices.Models
{

    public class WebSearchDto
    {
        public string _type { get; set; }
        public Querycontext QueryContext { get; set; }
        public Webpages WebPages { get; set; }
        public Relatedsearches RelatedSearches { get; set; }
        public Rankingresponse RankingResponse { get; set; }
    }

    public class Querycontext
    {
        public string OriginalQuery { get; set; }
    }

    public class Webpages
    {
        public string WebSearchUrl { get; set; }
        public int TotalEstimatedMatches { get; set; }
        public Value[] Value { get; set; }
        public bool SomeResultsRemoved { get; set; }
    }

    public class Value
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public About[] About { get; set; }
        public bool IsFamilyFriendly { get; set; }
        public string DisplayUrl { get; set; }
        public string Snippet { get; set; }
        public DateTime DateLastCrawled { get; set; }
        public string Language { get; set; }
    }

    public class About
    {
        public string Name { get; set; }
    }

    public class Relatedsearches
    {
        public string Id { get; set; }
        public Value1[] Value { get; set; }
    }

    public class Value1
    {
        public string Text { get; set; }
        public string DisplayText { get; set; }
        public string WebSearchUrl { get; set; }
    }

    public class Rankingresponse
    {
        public Mainline Mainline { get; set; }
        public Sidebar Sidebar { get; set; }
    }

    public class Mainline
    {
        public Item[] Items { get; set; }
    }

    public class Item
    {
        public string AnswerType { get; set; }
        public int ResultIndex { get; set; }
        public Value2 Value { get; set; }
    }

    public class Value2
    {
        public string Id { get; set; }
    }

    public class Sidebar
    {
        public Item1[] Items { get; set; }
    }

    public class Item1
    {
        public string AnswerType { get; set; }
        public Value3 Value { get; set; }
    }

    public class Value3
    {
        public string Id { get; set; }
    }

}
