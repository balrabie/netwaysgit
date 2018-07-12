using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnetcore.Data
{
    public class ContactUs
    {
        public ContactUs()
        {
            this.Locations = new HashSet<Location>();
        }

        public int ID { get; set; }

        public string Email { get; set; }

        public string WorkHours { get; set; }

        public virtual ICollection<Location> Locations { get; set; }

        public virtual ICollection<SocialMediaAccount> SocialMediaAccounts { get; set; }
    }
}