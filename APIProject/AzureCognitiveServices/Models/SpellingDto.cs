﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCognitiveServices.Models
{
    public class SpellingDto
    {
        public string Word { get; set; }

        public List<string> Suggestions { get; set; }
    }
}
