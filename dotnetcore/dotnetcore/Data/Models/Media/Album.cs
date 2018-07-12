using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcore.Data
{
    public class Album : Item
    {
        public byte[] CoverImage { get; set; }

        public string Details { get; set; }
        
    }
}