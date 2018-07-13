using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace dotnetcore.Data
{
    public class AwardDto : ItemDto
    {
        public byte[] Logo { get; set; }
        
        public int Number { get; set; }
    }
}