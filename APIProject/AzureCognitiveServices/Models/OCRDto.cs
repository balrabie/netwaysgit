using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveServices.Models
{
    public class OCRDto
    {
        public string Language { get; set; }

        public List<string> Lines { get; set; }
    }
}
