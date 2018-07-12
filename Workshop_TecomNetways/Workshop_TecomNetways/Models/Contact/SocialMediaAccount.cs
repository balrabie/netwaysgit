using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workshop_TecomNetways.Models
{
    public class SocialMediaAccount : Item
    {
        public string URL { get; set; }

        public int ContactUsID { get; set; }

        public virtual ContactUs ContactUs { get; set; }
        
    }
}