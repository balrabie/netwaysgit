using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveServices.Models
{
    public class ImageAnalysisDto
    {
        public List<Category> Categories { get; set; }

        public List<string> Tags { get; set; }

        public List<string> Comments { get; set; }

        public List<string> DominantColors { get; set; }
    }

    public struct Category
    {
        public string Name { get; set; }

        public double Confidence { get; set; }
    }
}
