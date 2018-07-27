using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveServices.Models
{
    public class TranslationDto
    {
        public string OriginalText { get; set; }

        public string FromLanguage { get; set; }

        public List<string> ToLanguage { get; set; }

        public List<string> TranslatedText { get; set; }

    }
}
