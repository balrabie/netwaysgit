using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcore.Data
{
    public class SocialMediaAccountDto : ItemDto
    {
        public string URL { get; set; }

        public int ContactUsID { get; set; }

        //public virtual ContactUs ContactUs { get; set; }
        
    }
}