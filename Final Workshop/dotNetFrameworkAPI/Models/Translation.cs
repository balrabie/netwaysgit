using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotNetFrameworkAPI.Models
{
    public class Translation 
    {
        public string FromLanguage { get; set; }

        public string[] ToLanguage { get; set; }

        public string OriginalText { get; set; }

        public string[] TranslatedText { get; set; }
    }
}