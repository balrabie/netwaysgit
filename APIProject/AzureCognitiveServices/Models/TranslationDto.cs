using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveServices.Models
{
    public class TranslationDto
    {
        public string FromLanguage { get; set; }

        public string[] ToLanguage { get; set; }

        public string OriginalText { get; set; }

        public string[] TranslatedText { get; set; }

    }
}
