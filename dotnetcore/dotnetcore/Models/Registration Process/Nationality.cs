using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Workshop_TecomNetways.Models
{
    public class Nationality
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}