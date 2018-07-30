using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIProject
{

    public struct TranslationInput
    {
        public string Text { get; set; }

        public List<string> To { get; set; }
    }

    public struct Input
    {
        public string Text { get; set; }
    }

    public struct Path
    {
        public string Target { get; set; }
    }

}
