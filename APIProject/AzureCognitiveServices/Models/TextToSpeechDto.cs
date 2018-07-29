using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveServices.Models
{
    public class TextToSpeechDto
    {
        public string Text { get; set; }
        public byte[] BytesData { get; set; }
    }
}
