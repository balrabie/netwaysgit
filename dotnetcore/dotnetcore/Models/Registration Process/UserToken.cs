using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Workshop_TecomNetways.Models
{
    public class UserToken
    {
        public int ID { get; set; }
        public DateTime Expiry { get; set; }
        public bool TokenIsUsed { get; set; }
        public string Token { get; set; } // those should not be set manually
        public int UserID { get; set; }
        public string Email { get; set; } // those should not be set manually
        public virtual User User { get; set; }   
    }
}